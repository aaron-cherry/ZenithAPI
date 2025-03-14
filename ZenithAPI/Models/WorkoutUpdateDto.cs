using System.ComponentModel.DataAnnotations;

namespace ZenithAPI.Models
{
    public class WorkoutUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
