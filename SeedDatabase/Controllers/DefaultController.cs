using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok("Hello world!");
        }
    }
}