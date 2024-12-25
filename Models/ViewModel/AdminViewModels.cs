using System.Collections.Generic;

namespace IsmagiLearningApp.Models.ViewModels
{
    // This class handles creating new programming languages with their difficulties
    public class CreateLanguageViewModel
    {
        public ProgrammingLanguage Language { get; set; }
        public List<Difficulty> AvailableDifficulties { get; set; }
    }

    // This class organizes levels by their difficulties for the management interface
    public class ManageLevelsViewModel
    {
        public ProgrammingLanguage Language { get; set; }
        public Dictionary<string, List<Level>> LevelsByDifficulty { get; set; }

    }
}