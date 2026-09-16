using System.Security.Cryptography.X509Certificates;

namespace ZenithAPI.Models
{
    public class ExerciseLogDto
    {
        public int Id { get; set; }
        public int SetNumber { get; set; }
        public DateTime CompletedAt { get; set; }
        public Dictionary<string, double> Metrics { get; set; } = new();
    }

    public class ExerciseLogCreateDto
    {
        public int SetNumber { get; set; }
        public Dictionary<string, double> Metrics { get; set; } = new();
    }

    public class ExerciseLogUpdateDto
    {
        public int SetNumber { get; set; }
        public Dictionary<string, double> Metrics { get; set; } = new();
    }
}
