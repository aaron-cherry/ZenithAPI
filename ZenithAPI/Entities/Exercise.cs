namespace ZenithAPI.Entities
{
    public class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
    }
}