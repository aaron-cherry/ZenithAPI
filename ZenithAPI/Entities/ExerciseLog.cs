using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

namespace ZenithAPI.Entities
{
    public class ExerciseLog
    {
        public int Id { get; set; }

        public int WorkoutExerciseId { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; } = null!;

        public int SetNumber { get; set; }

        public DateTime CompletedAt { get; set; }

        //Maps to a native PostgreSQL jsonb column
        [Column(TypeName = "jsonb")]
        public Dictionary<string, double> Metrics { get; set; } = new();
    }
}
