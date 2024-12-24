using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IsmagiLearningApp.Models
{
    public class Difficulty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public int ProgrammingLanguageId { get; set; }
        public virtual ProgrammingLanguage ProgrammingLanguage { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<Level> Levels { get; set; }

        // Constructor to initialize collections
        public Difficulty()
        {
            Levels = new HashSet<Level>();
        }
    }

}