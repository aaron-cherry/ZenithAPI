using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithAPI.Data;
using ZenithAPI.Entities;
using ZenithAPI.Models;

namespace ZenithAPI.Controllers
{
    [Route("api/workouts/{workoutId}/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ZenithDbContext _context;
        private readonly ILogger<ExercisesController> _logger;

        public ExercisesController(ZenithDbContext context, ILogger<ExercisesController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ExerciseDto>> GetExercises(int workoutId)
        {
            var workoutExists = _context.Workouts.Any(w => w.Id == workoutId);
            if (!workoutExists)
            {
                _logger.LogInformation($"The workout with ID {workoutId} could not be found");
                return NotFound();
            }

            var exercises = _context.WorkoutExercises
                .Where(we => we.WorkoutId == workoutId)
                .Include(we => we.Exercise)
                .Select(we => new ExerciseDto
                {
                    Id = we.Exercise.Id,
                    Name = we.Exercise.Name
                })
                .ToList();

            return Ok(exercises);
        }

        [HttpGet("{exerciseId}", Name = "GetExercise")]
        public ActionResult<ExerciseDto> GetExercise(int workoutId, int exerciseId)
        {
            var workoutExercise = _context.WorkoutExercises
                .Where(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId)
                .Include(we => we.Exercise)
                .FirstOrDefault();

            if (workoutExercise == null) return NotFound();

            var exerciseDto = new ExerciseDto
            {
                Id = workoutExercise.Exercise.Id,
                Name = workoutExercise.Exercise.Name
            };

            return Ok(exerciseDto);
        }

        [HttpPost]
        public ActionResult<ExerciseDto> CreateExercise(int workoutId, [FromBody] ExerciseCreateDto exerciseDto)
        {
            var workout = _context.Workouts.FirstOrDefault(w => w.Id == workoutId);
            if (workout == null) return NotFound();

            var exerciseEntity = _context.Exercises.FirstOrDefault(e => e.Name == exerciseDto.Name);
            if (exerciseEntity == null)
            {
                exerciseEntity = new Exercise { Name = exerciseDto.Name };
                _context.Exercises.Add(exerciseEntity);
                _context.SaveChanges();
            }

            var existingLink = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseEntity.Id);

            if (existingLink == null)
            {
                var workoutExercise = new WorkoutExercise
                {
                    WorkoutId = workoutId,
                    ExerciseId = exerciseEntity.Id
                };
                _context.WorkoutExercises.Add(workoutExercise);
                _context.SaveChanges();
            }

            var exerciseToReturn = new ExerciseDto
            {
                Id = exerciseEntity.Id,
                Name = exerciseEntity.Name
            };

            return CreatedAtRoute("GetExercise",
                new
                {
                    workoutId,
                    exerciseId = exerciseToReturn.Id
                },
                exerciseToReturn);
        }

        [HttpDelete("{exerciseId}")]
        public ActionResult DeleteExercise(int workoutId, int exerciseId)
        {
            var workoutExercise = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null) return NotFound();

            _context.WorkoutExercises.Remove(workoutExercise);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPut("{exerciseId}")]
        public ActionResult UpdateExercise(int workoutId, int exerciseId, [FromBody] ExerciseUpdateDto exercise)
        {
            var workoutExercise = _context.WorkoutExercises
                .Where(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId)
                .Include(we => we.Exercise)
                .FirstOrDefault();

            if (workoutExercise == null) return NotFound();

            workoutExercise.Exercise.Name = exercise.Name;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{exerciseId}")]
        public ActionResult PartiallyUpdateExercise(int workoutId, int exerciseId, JsonPatchDocument<ExerciseUpdateDto> patchDocument)
        {
            var workoutExercise = _context.WorkoutExercises
                .Where(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId)
                .Include(we => we.Exercise)
                .FirstOrDefault();

            if (workoutExercise == null) return NotFound();

            var exerciseToPatch = new ExerciseUpdateDto
            {
                Name = workoutExercise.Exercise.Name
            };

            patchDocument.ApplyTo(exerciseToPatch, ModelState);

            if (!ModelState.IsValid || !TryValidateModel(exerciseToPatch))
            {
                return BadRequest(ModelState);
            }

            workoutExercise.Exercise.Name = exerciseToPatch.Name;
            _context.SaveChanges();

            return NoContent();
        }
    }
}