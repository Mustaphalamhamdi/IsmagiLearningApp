using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace IsmagiLearningApp.Models
{
    public class UserProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int LevelId { get; set; }

        [ForeignKey("LevelId")]
        public Level Level { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime? CompletionDate { get; set; }

        public int AttemptCount { get; set; } = 0;
    }
}