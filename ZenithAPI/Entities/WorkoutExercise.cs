using System.Security.Cryptography.X509Certificates;

namespace ZenithAPI.Entities
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }
        public Exercise Exercise { get; set; }
        public int OrderIndex { get; set; }
        public int? RestTimeSeconds { get; set; }
    }
}