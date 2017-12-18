using Microsoft.AspNetCore.Mvc;

namespace SeedDatabase.Controllers
{
    [Route("api/[controller]")]
    public class DefaultController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world:");
        }
    }
}