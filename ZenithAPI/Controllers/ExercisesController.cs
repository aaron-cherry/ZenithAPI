using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPost]
        public ActionResult<ExerciseDto> CreateExercise(int workoutId, [FromBody] ExerciseCreateDto exercise)
        {
            var workout = SearchWorkout(workoutId);
            if (workout == null) return NotFound();

            var maxExerciseId = workout.Exercises.Max(i => i.Id);

            var finalExercise = new ExerciseDto()
            {
                Id = ++maxExerciseId,
                Name = exercise.Name
            };

            workout.Exercises.Add(finalExercise);
            return CreatedAtRoute("GetExercise",
                new
                {
                    workoutId = workoutId,
                    exerciseId = finalExercise
                },
                finalExercise);
        }

        [HttpDelete("{exerciseId}")]
        public ActionResult DeleteExercise(int workoutId, int exerciseId)
        {
            var workout = SearchWorkout(workoutId);
            if (workout == null) return NotFound();
            var exercise = workout.Exercises.Where(e => e.Id == exerciseId).FirstOrDefault();
            if (exercise == null) return NotFound();

            workout.Exercises.Remove(exercise);
            return NoContent();
        }

        [HttpPut("{exerciseId}")]
        public ActionResult UpdateExercise(int workoutId, int exerciseId, ExerciseUpdateDto exercise)
        {
            var originalExercise = SearchExercise(workoutId, exerciseId);
            if (originalExercise == null) return NotFound();
            originalExercise.Name = exercise.Name;
            return NoContent();

        }

        [HttpPatch("{exerciseId}")]
        public ActionResult PartiallyUpdateExercise(int workoutId, int exerciseId, JsonPatchDocument<ExerciseUpdateDto> patchDocument)
        {
            var originalExercsie = SearchExercise(workoutId, exerciseId);
            if (originalExercsie == null) return NotFound();

            var exerciseToPatch = new ExerciseUpdateDto
            {
                Name = originalExercsie.Name
            };

            patchDocument.ApplyTo(exerciseToPatch, ModelState);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(exerciseToPatch))
            {
                return BadRequest(ModelState);
            }

            originalExercsie.Name = exerciseToPatch.Name;
            return NoContent();
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
