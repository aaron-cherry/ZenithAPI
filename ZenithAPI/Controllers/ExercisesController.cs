using Microsoft.AspNetCore.Mvc;
using ZenithAPI.Models;

namespace ZenithAPI.Controllers
{
    [Route("api/workouts/{workoutId}/[controller]")]
    [ApiController]
    public class ExercisesController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<ExerciseDto>> GetExerices(int workoutId)
        {
            var workout = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();
            if (workout == null)
            {
                return NotFound();
            }
            return Ok(workout.Exercises);
        }

        [HttpGet("{exerciseId}", Name = "GetExercise")]
        public ActionResult<ExerciseDto> GetExercise(int workoutId, int exerciseId)
        {
            var exercise = SearchExercise(workoutId, exerciseId);
            if (exercise == null) return NotFound();
            return Ok(exercise);

        }

        private WorkoutDto? SearchWorkout(int workoutId)
        {
            var workout = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();
            if (workout == null) return null;
            return workout;
        }

        private ExerciseDto? SearchExercise(int workoutId, int exerciseId)
        {
            var workout = SearchWorkout(workoutId);
            if (workout == null) return null;
            var exercise = workout.Exercises.Where(e => e.Id == exerciseId).FirstOrDefault();
            if (exercise == null) return null;
            return exercise;
        }
    }
}
