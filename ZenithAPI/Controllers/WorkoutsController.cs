using Microsoft.AspNetCore.Mvc;

namespace ZenithAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetWorkouts()
        {
            return Ok(WorkoutsDataStore.Instance.Workouts);
        }
    }
}
