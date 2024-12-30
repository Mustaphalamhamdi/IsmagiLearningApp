using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IsmagiLearningApp.Data;
using IsmagiLearningApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IsmagiLearningApp.Controllers
{
    [Authorize]
    public class UserDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserDashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }

            // Load programming languages with their difficulties
            var languages = await _context.ProgrammingLanguages
                .Include(p => p.DifficultyLevels)
                .OrderBy(l => l.DisplayOrder)
                .ToListAsync();

            return View(languages);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProgress([FromBody] SaveProgressRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            var progress = await _context.UserProgress
                .FirstOrDefaultAsync(up => up.UserId == user.Id && up.LevelId == request.LevelId);

            if (progress == null)
            {
                progress = new UserProgress
                {
                    UserId = user.Id,
                    LevelId = request.LevelId,
                    StartDate = DateTime.Now,
                    AttemptCount = 1
                };
                _context.UserProgress.Add(progress);
            }
            else
            {
                progress.AttemptCount++;
            }

            if (request.IsCompleted && !progress.IsCompleted)
            {
                progress.IsCompleted = true;
                progress.CompletionDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        public class SaveProgressRequest
        {
            public int LevelId { get; set; }
            public bool IsCompleted { get; set; }
        }

        public async Task<IActionResult> Difficulties(int languageId)
        {
            // Load the programming language with its difficulties
            var language = await _context.ProgrammingLanguages
                .Include(pl => pl.DifficultyLevels) // Include the difficulties
                .FirstOrDefaultAsync(pl => pl.Id == languageId);

            if (language == null)
            {
                return NotFound("Programming language not found.");
            }

            // Store the language name for display in the view
            ViewBag.LanguageName = language.Name;
            return View(language.DifficultyLevels);
        }

        public async Task<IActionResult> Levels(int difficultyId)
        {
            // First, load the difficulty to get its details
            var difficulty = await _context.DifficultyLevels
                .Include(d => d.ProgrammingLanguage)
                .FirstOrDefaultAsync(d => d.Id == difficultyId);

            if (difficulty == null)
            {
                return NotFound("Difficulty not found.");
            }

            // Now load all levels for this difficulty
            var levels = await _context.Levels
                .Where(l => l.DifficultyId == difficultyId) // Use DifficultyId instead of Id
                .OrderBy(l => l.OrderIndex)
                .ToListAsync();

            // Store difficulty and language information for the view
            ViewBag.DifficultyName = difficulty.Name;
            ViewBag.LanguageName = difficulty.ProgrammingLanguage.Name;

            return View(levels);
        }

        public async Task<IActionResult> LevelDetails(int levelId)
        {
            // Load the level with its related difficulty and programming language
            var level = await _context.Levels
                .Include(l => l.Difficulty)
                .Include(l => l.ProgrammingLanguage)
                .FirstOrDefaultAsync(l => l.Id == levelId);

            if (level == null)
            {
                return NotFound("Level not found.");
            }

            return View(level);
        }
        public async Task<IActionResult> Progress()
        {
            var user = await _userManager.GetUserAsync(User);
            var progress = await _context.UserProgress
                .Include(up => up.Level)
                    .ThenInclude(l => l.ProgrammingLanguage)
                .Include(up => up.Level.Difficulty)
                .Where(up => up.UserId == user.Id)
                .OrderByDescending(up => up.CompletionDate)
                .ToListAsync();

            return View(progress);
        }
        public async Task<IActionResult> GetNextLevel(int currentLevelId)
        {
            var currentLevel = await _context.Levels
                .Include(l => l.Difficulty)
                .Include(l => l.ProgrammingLanguage)
                .FirstOrDefaultAsync(l => l.Id == currentLevelId);

            if (currentLevel == null)
                return NotFound();

            // Find the next level in the same difficulty
            var nextLevel = await _context.Levels
                .Where(l => l.DifficultyId == currentLevel.DifficultyId &&
                            l.ProgrammingLanguageId == currentLevel.ProgrammingLanguageId &&
                            l.OrderIndex > currentLevel.OrderIndex)
                .OrderBy(l => l.OrderIndex)
                .FirstOrDefaultAsync();

            if (nextLevel == null)
            {
                // If no next level in current difficulty, try to find first level of next difficulty
                var nextDifficulty = await _context.DifficultyLevels
                    .Where(d => d.ProgrammingLanguageId == currentLevel.ProgrammingLanguageId &&
                               d.DisplayOrder > currentLevel.Difficulty.DisplayOrder)
                    .OrderBy(d => d.DisplayOrder)
                    .FirstOrDefaultAsync();

                if (nextDifficulty != null)
                {
                    nextLevel = await _context.Levels
                        .Where(l => l.DifficultyId == nextDifficulty.Id)
                        .OrderBy(l => l.OrderIndex)
                        .FirstOrDefaultAsync();
                }
            }

            if (nextLevel == null)
            {
                // No more levels available
                return Json(new
                {
                    success = true,
                    hasNextLevel = false,
                    message = "Congratulations! You've completed all levels!"
                });
            }

            return Json(new
            {
                success = true,
                hasNextLevel = true,
                nextLevelId = nextLevel.Id
            });
        }
        
    }
}