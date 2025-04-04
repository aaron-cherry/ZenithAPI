using System.ComponentModel.DataAnnotations;

namespace ZenithAPI.Models
{
    public class WorkoutCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<ExerciseCreateDto> Exercises = new List<ExerciseCreateDto>();
    }
}
