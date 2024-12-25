using IsmagiLearningApp.Models;
using System.Collections.Generic;

namespace IsmagiLearningApp.Models.ViewModels
{
    public class LevelsViewModel
    {
        // This property stores information about the programming language being learned
        public ProgrammingLanguage Language { get; set; }

        // This property holds the current difficulty level (Beginner, Advanced, or Master)
        public string Difficulty { get; set; }

        // This list contains all the levels available for the selected language and difficulty
        public List<Level> Levels { get; set; }

        // We can add a helper property to show progress
        public int TotalLevels => Levels?.Count ?? 0;
    }
}