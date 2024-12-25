using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsmagiLearningApp.Models
{
    public class Difficulty
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsAvailable { get; set; }

        // Foreign key for ProgrammingLanguage
        public int ProgrammingLanguageId { get; set; }

        // Navigation properties
        [ForeignKey("ProgrammingLanguageId")]
        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }

        // Add this navigation property
        public virtual ICollection<Level> Levels { get; set; }

        public Difficulty()
        {
            Levels = new List<Level>();
        }
    }

}