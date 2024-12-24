using IsmagiLearningApp.Data;
using IsmagiLearningApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  // Add this for [Authorize] attribute

public class LevelController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;  // Add this for user management

    public LevelController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [Authorize]  // This ensures only logged-in users can access levels
    public async Task<IActionResult> Details(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var level = await _context.Levels
            .Include(l => l.ProgrammingLanguage)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (level == null)
        {
            return NotFound();
        }

        // Get or create progress record for this user and level
        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == user.Id && up.LevelId == id);

        if (progress == null)
        {
            progress = new UserProgress
            {
                UserId = user.Id,
                LevelId = id,
                StartDate = DateTime.Now,
                AttemptCount = 0,
                IsCompleted = false
            };
            _context.UserProgress.Add(progress);
            await _context.SaveChangesAsync();
        }

        // Pass both level and progress to the view
        ViewBag.Progress = progress;
        return View(level);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CheckSolution(int levelId, string solution)
    {
        var user = await _userManager.GetUserAsync(User);
        var level = await _context.Levels.FindAsync(levelId);
        var progress = await _context.UserProgress
            .FirstOrDefaultAsync(up => up.UserId == user.Id && up.LevelId == levelId);

        progress.AttemptCount++;

        // Check if solution matches expected solution
        bool isCorrect = level.ExpectedSolution.Trim() == solution.Trim();

        if (isCorrect && !progress.IsCompleted)
        {
            progress.IsCompleted = true;
            progress.CompletionDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();

        return Json(new
        {
            isCorrect = isCorrect,
            attemptCount = progress.AttemptCount,
            showSolution = progress.AttemptCount >= 3  // Show solution after 3 attempts
        });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifySolution(int levelId, string userSolution)
    {
        var level = await _context.Levels.FindAsync(levelId);
        if (level == null)
        {
            return NotFound();
        }

        // Vérifiez si la solution de l'utilisateur correspond à la solution attendue
        level.IsSolutionCorrect = userSolution.Trim() == level.ExpectedSolution.Trim();

        // Rechargez les difficultés pour la vue
        var language = await _context.ProgrammingLanguages
            .Include(p => p.DifficultyLevels)
            .FirstOrDefaultAsync(p => p.Id == level.ProgrammingLanguageId);

        ViewBag.Language = language;
        ViewBag.Difficulties = language.DifficultyLevels
            .Where(d => d.IsAvailable)
            .OrderBy(d => d.DisplayOrder)
            .ToList();

        return View("EditLevel", level);
    }
    [HttpPost]
    public async Task<IActionResult> VerifySolutionAjax(int levelId, string userSolution)
    {
        var level = await _context.Levels.FindAsync(levelId);
        if (level == null)
        {
            return Json(new { success = false, message = "Niveau non trouvé." });
        }

        bool isSolutionCorrect = userSolution.Trim() == level.ExpectedSolution.Trim();
        return Json(new { success = true, isSolutionCorrect });
    }


}