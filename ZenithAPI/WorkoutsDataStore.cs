using ZenithAPI.Models;

namespace ZenithAPI
{
    public class WorkoutsDataStore
    {
        public List<WorkoutDto> Workouts {  get; set; }
        public static WorkoutsDataStore Instance { get; set; } = new WorkoutsDataStore();

        public WorkoutsDataStore()
        {
            Workouts = new List<WorkoutDto>()
            {
                new WorkoutDto()
                {
                    Id = 1,
                    Name = "Push",
                    Exercises = new List<ExerciseDto>()
                    {
                        new ExerciseDto()
                        {
                            Id = 1,
                            Name = "Dumbell Bench Press"
                        },
                        new ExerciseDto()
                        {
                            Id = 2,
                            Name = "Chest Flys"
                        },
                        new ExerciseDto()
                        {
                            Id = 3,
                            Name = "Overhead Press"
                        },
                        new ExerciseDto()
                        {
                            Id = 4,
                            Name = "Tricep Pushdown"
                        },
                    }
                },
                new WorkoutDto()
                {
                    Id = 2,
                    Name = "Pull",
                    Exercises = new List<ExerciseDto>()
                    {
                        new ExerciseDto()
                        {
                            Id = 1,
                            Name = "Cable Row"
                        },
                        new ExerciseDto()
                        {
                            Id = 2,
                            Name = "Lat Pulldown"
                        },
                        new ExerciseDto()
                        {
                            Id = 3,
                            Name = "Hammer Curl"
                        },
                        new ExerciseDto()
                        {
                            Id = 4,
                            Name = "Bicep Curl"
                        },
                    }
                },
                new WorkoutDto()
                {
                    Id = 3,
                    Name = "Legs",
                    Exercises = new List<ExerciseDto>()
                    {
                        new ExerciseDto()
                        {
                            Id = 1,
                            Name = "Romanian Deadlift"
                        },
                        new ExerciseDto()
                        {
                            Id = 2,
                            Name = "Bulgarian Split Squats"
                        },
                        new ExerciseDto()
                        {
                            Id = 3,
                            Name = "Hamstring Curl"
                        },
                        new ExerciseDto()
                        {
                            Id = 4,
                            Name = "Leg Extension"
                        },
                    }
                },
            };
        }
    }
}
