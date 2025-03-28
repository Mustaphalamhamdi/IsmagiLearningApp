﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IsmagiLearningApp.Models;
using IsmagiLearningApp.Models.ViewModels;
using IsmagiLearningApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IsmagiLearningApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Shows the main admin dashboard with overview statistics
        public async Task<IActionResult> Index()
        {
            var dashboardStats = new AdminDashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalLanguages = await _context.ProgrammingLanguages.CountAsync(),
                TotalLevels = await _context.Levels.CountAsync(),

                Languages = await _context.ProgrammingLanguages
                    .Include(p => p.DifficultyLevels)
                    .OrderBy(l => l.DisplayOrder)
                    .ToListAsync(),

                RecentActivities = await _context.UserProgress
                    .Include(up => up.Level)
                    .ThenInclude(l => l.ProgrammingLanguage)
                    .OrderByDescending(up => up.CompletionDate)
                    .Take(10)
                    .ToListAsync()
            };

            return View(dashboardStats);
        }

        // Displays the language management interface
        public async Task<IActionResult> ManageLanguages()
        {
            var languages = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .OrderBy(l => l.DisplayOrder)
                .ToListAsync();

            return View(languages);
        }

        // Shows the interface for creating a new programming language
        public IActionResult CreateLanguage()
        {
            return View(new ProgrammingLanguage());
        }

        // Handles the submission of a new programming language
        [HttpPost]
        public async Task<IActionResult> CreateLanguage(ProgrammingLanguage language)
        {
            if (ModelState.IsValid)
            {
                language.CreatedDate = DateTime.Now;
                _context.ProgrammingLanguages.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageDifficulties), new { languageId = language.Id });
            }
            return View(language);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLanguage(int id)
        {
            var language = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .Include(p => p.Levels)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (language == null)
            {
                return NotFound();
            }

            // Supprimer les niveaux associés
            _context.Levels.RemoveRange(language.Levels);

            // Supprimer les niveaux de difficulté associés
            _context.DifficultyLevels.RemoveRange(language.DifficultyLevels);

            // Supprimer le langage de programmation
            _context.ProgrammingLanguages.Remove(language);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Shows the interface for managing levels within a language
        public async Task<IActionResult> ManageLevels(int languageId)
        {
            // Load the programming language with its related difficulties and levels

            var language = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .Include(p => p.Levels)
                    .ThenInclude(l => l.Difficulty) // Include the Difficulty navigation property
                .FirstOrDefaultAsync(p => p.Id == languageId);

            if (language == null)
            {
                return NotFound();
            }

            // Create groups based on the Difficulty entity rather than a string
            var levels = language.Levels ?? new List<Level>();
            var viewModel = new ManageLevelsViewModel
            {
                Language = language,
                LevelsByDifficulty = levels
                    .GroupBy(l => l.Difficulty) // Group by the Difficulty entity
                    .ToDictionary(
                        g => g.Key.Name, // Use the difficulty name as the dictionary key
                        g => g.OrderBy(l => l.OrderIndex).ToList()
                    )
            };

            return View(viewModel);
        }
        // Show form to create a new level
        // GET: Shows the form to create a new level
        [HttpGet]
        public async Task<IActionResult> CreateLevel(int languageId)
        {
            // First, we fetch the programming language to make sure it exists
            var language = await _context.ProgrammingLanguages
                .FirstOrDefaultAsync(p => p.Id == languageId);

            if (language == null)
            {
                return NotFound("Programming language not found.");
            }

            // Then we fetch the difficulties available for this language
            var difficulties = await _context.DifficultyLevels
                .Where(d => d.ProgrammingLanguageId == languageId)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            // We store these in ViewBag to use them in our dropdown
            ViewBag.Language = language;
            ViewBag.Difficulties = new SelectList(difficulties, "Id", "Name");

            // Create a new Level with the languageId already set
            var level = new Level
            {
                ProgrammingLanguageId = languageId,
                OrderIndex = 1,  // Default value for order
                VerificationPattern = string.Empty // Default value for required property
            };

            return View(level);
        }

        // POST: Handles the submission of the new level form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLevel(Level level)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Levels.Add(level);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageLevels), new { languageId = level.ProgrammingLanguageId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating level: " + ex.Message);
                }
            }


            // If we get here, something went wrong. Reload the form data
            var language = await _context.ProgrammingLanguages
                .FirstOrDefaultAsync(p => p.Id == level.ProgrammingLanguageId);
            var difficulties = await _context.DifficultyLevels
                .Where(d => d.ProgrammingLanguageId == level.ProgrammingLanguageId)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();

            ViewBag.Language = language;
            ViewBag.Difficulties = new SelectList(difficulties, "Id", "Name");

            return View(level);
        }

        public async Task<IActionResult> EditLevel(int id)
        {
            var level = await _context.Levels
                .Include(l => l.ProgrammingLanguage)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (level == null)
                return NotFound();

            var language = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .FirstOrDefaultAsync(p => p.Id == level.ProgrammingLanguageId);

            if (language == null || language.DifficultyLevels == null)
                return NotFound();

            ViewBag.Language = language;
            ViewBag.Difficulties = language.DifficultyLevels
                .Where(d => d.IsAvailable)
                .OrderBy(d => d.DisplayOrder)
                .ToList();

            return View(level);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLevel(Level level)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(level);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(ManageLevels), new { languageId = level.ProgrammingLanguageId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Levels.Any(e => e.Id == level.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            var language = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .FirstOrDefaultAsync(p => p.Id == level.ProgrammingLanguageId);

            if (language == null || language.DifficultyLevels == null)
                return NotFound();

            ViewBag.Language = language;
            ViewBag.Difficulties = language.DifficultyLevels
                .Where(d => d.IsAvailable)
                .OrderBy(d => d.DisplayOrder)
                .ToList();

            return View(level);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteLevel(int id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level == null)
                return NotFound();

            var languageId = level.ProgrammingLanguageId;
            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ManageLevels), new { languageId });
        }
        // Show difficulty levels for a specific programming language
        public async Task<IActionResult> ManageDifficulties(int languageId)
        {
            var language = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .FirstOrDefaultAsync(p => p.Id == languageId);

            if (language == null)
                return NotFound();

            return View(language);
        }

        // Show form to create a new difficulty level
        [HttpGet]
        public async Task<IActionResult> CreateDifficulty(int languageId)
        {
            var language = await _context.ProgrammingLanguages.FindAsync(languageId);
            if (language == null)
            {
                return NotFound();
            }

            ViewData["Language"] = language;
            var difficulty = new Difficulty
            {
                ProgrammingLanguageId = languageId,
                IsAvailable = true,
                DisplayOrder = 1  // Default value
            };
            return View(difficulty);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDifficulty(Difficulty difficulty)
        {
            // Remove the ProgrammingLanguage validation since we're using ProgrammingLanguageId
            ModelState.Remove("ProgrammingLanguage");

            try
            {
                if (ModelState.IsValid)
                {
                    // Log or check the received data
                    Console.WriteLine($"Received difficulty: Name={difficulty.Name}, ProgrammingLanguageId={difficulty.ProgrammingLanguageId}");

                    difficulty.IsAvailable = true;
                    _context.DifficultyLevels.Add(difficulty);
                    await _context.SaveChangesAsync();

                    // After saving, try to retrieve it to confirm it was saved
                    var savedDifficulty = await _context.DifficultyLevels
                        .FirstOrDefaultAsync(d => d.Name == difficulty.Name &&
                                                d.ProgrammingLanguageId == difficulty.ProgrammingLanguageId);

                    if (savedDifficulty != null)
                    {
                        return RedirectToAction(nameof(ManageDifficulties),
                            new { languageId = difficulty.ProgrammingLanguageId });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to save the difficulty level.");
                    }
                }
                else
                {
                    // Log what validation errors occurred
                    foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine($"Model Error: {modelError.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving difficulty: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the difficulty level.");
            }

            // If we get here, something failed; reload the language for the view
            var language = await _context.ProgrammingLanguages.FindAsync(difficulty.ProgrammingLanguageId);
            ViewData["Language"] = language;
            return View(difficulty);
        }
        // Handles updating difficulty level availability
        [HttpPost]
        public async Task<IActionResult> UpdateDifficultyAvailability(int difficultyId, bool isAvailable)
        {
            var difficulty = await _context.DifficultyLevels.FindAsync(difficultyId);
            if (difficulty == null)
            {
                return NotFound();
            }

            difficulty.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // Edit difficulty level
        public async Task<IActionResult> EditDifficulty(int id)
        {
            var difficulty = await _context.DifficultyLevels
                .Include(d => d.ProgrammingLanguage)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (difficulty == null)
                return NotFound();

            return View(difficulty);
        }

        [HttpPost]
        public async Task<IActionResult> EditDifficulty(Difficulty difficulty)
        {
            if (ModelState.IsValid)
            {
                _context.Update(difficulty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageDifficulties), new { languageId = difficulty.ProgrammingLanguageId });
            }
            return View(difficulty);
        }
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        // Delete difficulty level
        [HttpPost]
        public async Task<IActionResult> DeleteDifficulty(int id)
        {
            var difficulty = await _context.DifficultyLevels
                .Include(d => d.Levels)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (difficulty == null)
            {
                return NotFound();
            }

            // Supprimer les niveaux associés
            _context.Levels.RemoveRange(difficulty.Levels);

            // Supprimer la difficulté
            _context.DifficultyLevels.Remove(difficulty);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}