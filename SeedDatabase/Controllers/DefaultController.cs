using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SeedDatabase.Controllers
{
    [Route("dev/seed")]
    public class SeedDatabaseController : Controller
    {
        private readonly UserManager<Security.Data.Entities.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world!");
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}