using System.Collections.Generic;
using IsmagiLearningApp.Models;
namespace IsmagiLearningApp.Models.ViewModel
{
    public class ManageLevelsViewModel
    {
        // The programming language whose levels we're managing
        public ProgrammingLanguage Language { get; set; }

        // A dictionary that groups levels by difficulty names
        // The string key is the difficulty name (like "Beginner", "Advanced")
        // The List<Level> value contains all levels for that difficulty
        public Dictionary<string, List<Level>> LevelsByDifficulty { get; set; }

        // Constructor to initialize the dictionary to prevent null reference exceptions
        public ManageLevelsViewModel()
        {
            LevelsByDifficulty = new Dictionary<string, List<Level>>();
        }
    }
}
