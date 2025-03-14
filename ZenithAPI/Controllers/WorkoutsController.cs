using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ZenithAPI.Models;

namespace ZenithAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetWorkouts()
        {
            return Ok(WorkoutsDataStore.Instance.Workouts);
        }

        [HttpGet("{id}", Name = "GetWorkout")]
        public ActionResult GetWorkout(int id)
        {
            var workout = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == id).FirstOrDefault();
            if (workout == null)
            {
                return NotFound();
            }
            return Ok(workout);
        }

        [HttpPost]
        public ActionResult CreateWorkout([FromBody] WorkoutCreateDto workout)
        {
            if (workout == null) return BadRequest();

            int maxId = WorkoutsDataStore.Instance.Workouts.Max(w => w.Id);
            var finalWorkout = new WorkoutDto()
            {
                Id = ++maxId,
                Name = workout.Name
            };

            WorkoutsDataStore.Instance.Workouts.Add(finalWorkout);
            return CreatedAtRoute("GetWorkout", new
            {
                id = finalWorkout.Id
            },
            finalWorkout);
        }

        [HttpDelete("{workoutId}")]
        public ActionResult DeleteWorkout(int workoutId)
        {
            var workout = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();
            if (workout == null) return NotFound();
            WorkoutsDataStore.Instance.Workouts.Remove(workout);
            return Ok();
        }

        [HttpPut("{workoutId}")]
        public ActionResult UpdateWorkout(int workoutId, [FromBody] WorkoutCreateDto workout)
        {
            var workoutFromStore = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();
            if(workoutFromStore == null) return NotFound();

            workoutFromStore.Name = workout.Name;
            return NoContent();
        }

        [HttpPatch("{workoutId}")]
        public ActionResult PartiallyUpdateWorkout(int workoutId, JsonPatchDocument<WorkoutUpdateDto> patchDocument)
        {
            var workoutOriginal = WorkoutsDataStore.Instance.Workouts.Where(w => w.Id == workoutId).FirstOrDefault();
            if (workoutOriginal == null) return NotFound();

            var workoutToPatch = new WorkoutUpdateDto
            {
                Name = workoutOriginal.Name
            };

            patchDocument.ApplyTo(workoutToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(workoutToPatch))
            {
                return BadRequest(ModelState);
            }

            workoutOriginal.Name = workoutToPatch.Name;

            return NoContent();
        }
    }
}
