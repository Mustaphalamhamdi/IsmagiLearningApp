using System.Collections.Generic;
using IsmagiLearningApp.Models;

namespace IsmagiLearningApp.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        // Statistics for the dashboard
        public int TotalUsers { get; set; }
        public int TotalLanguages { get; set; }
        public int TotalLevels { get; set; }

        // Collections for display
        public List<ProgrammingLanguage> Languages { get; set; }
        public List<UserProgress> RecentActivities { get; set; }

        // Constructor to initialize collections
        public AdminDashboardViewModel()
        {
            Languages = new List<ProgrammingLanguage>();
            RecentActivities = new List<UserProgress>();
        }
    }
}