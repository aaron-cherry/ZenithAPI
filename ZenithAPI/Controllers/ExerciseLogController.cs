using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZenithAPI.Data;
using ZenithAPI.Entities;
using ZenithAPI.Models;

namespace ZenithAPI.Controllers
{
    [ApiController]
    [Route("api/workouts/{workoutId}/exercises/{exerciseId}/logs")]
    [Produces("application/json", "application/xml")]
    public class ExerciseLogsController : ControllerBase
    {
        private readonly ZenithDbContext _context;
        private readonly ILogger<ExerciseLogsController> _logger;

        public ExerciseLogsController(ZenithDbContext context, ILogger<ExerciseLogsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/workouts/1/exercises/2/logs
        [HttpGet]
        public ActionResult<IEnumerable<ExerciseLogDto>> GetLogs(int workoutId, int exerciseId)
        {
            var workoutExercise = _context.WorkoutExercises
                .Include(we => we.Logs)
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null)
            {
                _logger.LogInformation($"No link found for Workout {workoutId} and Exercise {exerciseId}");
                return NotFound();
            }

            var logsToReturn = workoutExercise.Logs
                .OrderBy(l => l.SetNumber)
                .Select(l => new ExerciseLogDto
                {
                    Id = l.Id,
                    SetNumber = l.SetNumber,
                    CompletedAt = l.CompletedAt,
                    Metrics = l.Metrics
                })
                .ToList();

            return Ok(logsToReturn);
        }

        // GET: api/workouts/1/exercises/2/logs/5
        [HttpGet("{logId}", Name = "GetLog")]
        public ActionResult<ExerciseLogDto> GetLog(int workoutId, int exerciseId, int logId)
        {
            var workoutExercise = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null) return NotFound();

            var log = _context.ExerciseLogs
                .FirstOrDefault(l => l.WorkoutExerciseId == workoutExercise.Id && l.Id == logId);

            if (log == null) return NotFound();

            var logDto = new ExerciseLogDto
            {
                Id = log.Id,
                SetNumber = log.SetNumber,
                CompletedAt = log.CompletedAt,
                Metrics = log.Metrics
            };

            return Ok(logDto);
        }

        // POST: api/workouts/1/exercises/2/logs
        [HttpPost]
        public ActionResult<ExerciseLogDto> CreateLog(int workoutId, int exerciseId, [FromBody] ExerciseLogCreateDto logDto)
        {
            var workoutExercise = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null) return NotFound();

            var logEntity = new ExerciseLog
            {
                WorkoutExerciseId = workoutExercise.Id,
                SetNumber = logDto.SetNumber,
                CompletedAt = DateTime.UtcNow,
                Metrics = logDto.Metrics
            };

            _context.ExerciseLogs.Add(logEntity);
            _context.SaveChanges();

            var logToReturn = new ExerciseLogDto
            {
                Id = logEntity.Id,
                SetNumber = logEntity.SetNumber,
                CompletedAt = logEntity.CompletedAt,
                Metrics = logEntity.Metrics
            };

            return CreatedAtRoute("GetLog",
                new
                {
                    workoutId,
                    exerciseId,
                    logId = logToReturn.Id
                },
                logToReturn);
        }

        // PUT: api/workouts/1/exercises/2/logs/5
        [HttpPut("{logId}")]
        public ActionResult UpdateLog(int workoutId, int exerciseId, int logId, [FromBody] ExerciseLogUpdateDto logDto)
        {
            var workoutExercise = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null) return NotFound();

            var logEntity = _context.ExerciseLogs
                .FirstOrDefault(l => l.WorkoutExerciseId == workoutExercise.Id && l.Id == logId);

            if (logEntity == null) return NotFound();

            logEntity.SetNumber = logDto.SetNumber;
            logEntity.Metrics = logDto.Metrics;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/workouts/1/exercises/2/logs/5
        [HttpDelete("{logId}")]
        public ActionResult DeleteLog(int workoutId, int exerciseId, int logId)
        {
            var workoutExercise = _context.WorkoutExercises
                .FirstOrDefault(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (workoutExercise == null) return NotFound();

            var logEntity = _context.ExerciseLogs
                .FirstOrDefault(l => l.WorkoutExerciseId == workoutExercise.Id && l.Id == logId);

            if (logEntity == null) return NotFound();

            _context.ExerciseLogs.Remove(logEntity);
            _context.SaveChanges();

            return NoContent();
        }
    }
}