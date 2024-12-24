using System.ComponentModel.DataAnnotations;

namespace IsmagiLearningApp.Models.Requests
{
    // This class handles requests when users want to run their code
    public class RunCodeRequest
    {
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Programming language must be specified")]
        public string Language { get; set; }
    }

    // This class handles requests when users submit their solution for checking
    public class CheckSolutionRequest
    {
        [Required(ErrorMessage = "Level ID is required")]
        public int LevelId { get; set; }

        [Required(ErrorMessage = "Solution code is required")]
        public string Solution { get; set; }
    }
}