using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;

namespace Barebone.Controllers
{
    public class DefaultController : ControllerBase
    {

        public IActionResult Index()
        {
            return this.View();
        }
    }
}