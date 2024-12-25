using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IsmagiLearningApp.Models
{
    public class ProgrammingLanguage
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DisplayOrder { get; set; }

        // Navigation properties for relationships
        public virtual ICollection<Difficulty> DifficultyLevels { get; set; }
        public virtual ICollection<Level> Levels { get; set; }

        // Constructor to initialize collections
        public ProgrammingLanguage()
        {
            DifficultyLevels = new List<Difficulty>();
            Levels = new List<Level>();
        }
    }
}