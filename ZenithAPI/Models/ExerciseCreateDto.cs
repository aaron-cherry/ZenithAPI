using System.ComponentModel.DataAnnotations;

namespace ZenithAPI.Models
{
    public class ExerciseCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
