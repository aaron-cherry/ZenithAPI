using System.ComponentModel.DataAnnotations;

namespace ZenithAPI.Models
{
    public class ExerciseUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
