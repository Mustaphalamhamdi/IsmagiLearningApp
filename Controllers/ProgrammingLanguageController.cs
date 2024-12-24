using Microsoft.AspNetCore.Mvc;
using IsmagiLearningApp.Data;
using IsmagiLearningApp.Models;

public class ProgrammingLanguageController : Controller
{
    private readonly ApplicationDbContext _context;

    // Constructor - gets database access when controller is created
    public ProgrammingLanguageController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Action to show list of programming languages
    public IActionResult Index()
    {
        var languages = _context.ProgrammingLanguages.ToList();
        return View(languages);
    }
}