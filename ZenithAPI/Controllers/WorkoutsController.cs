using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ZenithAPI.Data;
using ZenithAPI.Entities;
using ZenithAPI.Models;

namespace ZenithAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class WorkoutsController : ControllerBase
    {
        private readonly ZenithDbContext _context;

        public WorkoutsController(ZenithDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        [HttpGet]
        public ActionResult GetWorkouts()
        {
            return Ok(_context.Workouts.ToList());
        }

        [HttpGet("{id}", Name = "GetWorkout")]
        public ActionResult GetWorkout(int id)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == id);

            if (workout == null)
            {
                return NotFound();
            }
            return Ok(workout);
        }

        [HttpPost]
        public ActionResult CreateWorkout([FromBody] WorkoutCreateDto workoutDto)
        {
            if (workoutDto == null) return BadRequest();

            var workoutEntity = new Workout
            {
                Name = workoutDto.Name
            };

            _context.Workouts.Add(workoutEntity);
            _context.SaveChanges();

            var workoutToReturn = new WorkoutDto
            {
                Id = workoutEntity.Id,
                Name = workoutEntity.Name
            };

            return CreatedAtRoute("GetWorkout", new
            {
                id = workoutToReturn.Id
            },
            workoutToReturn);
        }

        [HttpDelete("{workoutId}")]
        public ActionResult DeleteWorkout(int workoutId)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == workoutId);

            if (workout == null) return NotFound();
            _context.Workouts.Remove(workout);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("{workoutId}")]
        public ActionResult UpdateWorkout(int workoutId, [FromBody] WorkoutUpdateDto workout)
        {
            var workoutFromDb = _context.Workouts.FirstOrDefault(w => w.Id == workoutId);

            if(workoutFromDb == null) return NotFound();
            
            workoutFromDb.Name = workout.Name;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{workoutId}")]
        public ActionResult PartiallyUpdateWorkout(int workoutId, JsonPatchDocument<WorkoutUpdateDto> patchDocument)
        {
            var workoutEntity = _context.Workouts.FirstOrDefault(w => w.Id == workoutId);
            if (workoutEntity == null) return NotFound();

            var workoutToPatch = new WorkoutUpdateDto
            {
                Name = workoutEntity.Name
            };

            patchDocument.ApplyTo(workoutToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(workoutToPatch))
            {
                return BadRequest(ModelState);
            }

            workoutEntity.Name = workoutToPatch.Name;
            _context.SaveChanges();

            return NoContent();
        }
    }
}
