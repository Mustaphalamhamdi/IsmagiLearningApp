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

        // First relationship - Programming Language
        public int ProgrammingLanguageId { get; set; }

        [ForeignKey("ProgrammingLanguageId")]
        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }

        // Second relationship - Difficulty Level
        public int DifficultyId { get; set; }

        [ForeignKey("DifficultyId")]
        public virtual Difficulty Difficulty { get; set; }

        [Required]
        public string InitialCode { get; set; }

        [Required]
        public string ExpectedSolution { get; set; }

        public string Hints { get; set; }

        public int OrderIndex { get; set; }
    }
}
