using IsmagiLearningApp.Models;
using System.Collections.Generic;

namespace IsmagiLearningApp.Models.ViewModels
{
    public class LanguageDifficultiesViewModel
    {
        // This property holds the programming language information
        public ProgrammingLanguage Language { get; set; }

        // This list contains information about each difficulty level available
        public List<DifficultyInfo> DifficultiesAvailable { get; set; }
    }

    // This class represents information about each difficulty level
    public class DifficultyInfo
    {
        // The name of the difficulty level (e.g., "Beginner", "Advanced", "Master")
        public string Name { get; set; }

        // A description explaining what this difficulty level means
        public string Description { get; set; }

        // Whether this difficulty level is currently available for the language
        public bool IsAvailable { get; set; }
    }
}