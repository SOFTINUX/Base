using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;

namespace Barebone.Controllers
{
    public class BareboneController : ControllerBase
    {

        public ActionResult Index()
        {
            return View(new IndexViewModelFactory().Create());
        }
    }
}