namespace ZenithAPI.Models
{
    public class WorkoutDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    }
}
