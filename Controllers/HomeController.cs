using IsmagiLearningApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IsmagiLearningApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // If user is logged in, redirect to dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "UserDashboard");
            }

            // Otherwise show the home page
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
