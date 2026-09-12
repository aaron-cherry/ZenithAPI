namespace ZenithAPI.Entities
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}