using Microsoft.EntityFrameworkCore;
using ZenithAPI.Entities;

namespace ZenithAPI.Data
{
    public class ZenithDbContext : DbContext
    {
        public ZenithDbContext(DbContextOptions<ZenithDbContext> options) : base(options)
        {
        }

        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
    }
}