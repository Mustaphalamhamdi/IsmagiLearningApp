using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace IsmagiLearningApp.Models
{
    public class Level
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        // Navigation property for Programming Language
        public int ProgrammingLanguageId { get; set; }

        [ForeignKey("ProgrammingLanguageId")]
        public ProgrammingLanguage ProgrammingLanguage { get; set; }

        // This connects to our difficulty level system
        public int DifficultyId { get; set; }  // Foreign Key to Difficulty

        [ForeignKey("DifficultyId")]
        public Difficulty Difficulty { get; set; }

        public string InitialCode { get; set; }

        public string ExpectedSolution { get; set; }

        public string Hints { get; set; }

        // Order within its difficulty level
        public int OrderIndex { get; set; }

        // Property to store the result of the solution verification
        [NotMapped]
        public bool IsSolutionCorrect { get; set; }
        public required string VerificationPattern { get; set; } // Ajoutez cette propriété

    }
}
