using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;

namespace Barebone.Controllers
{
    public class BareboneController : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViewModelFactory().Create());
        }
    }
}