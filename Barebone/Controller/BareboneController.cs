using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;

namespace Barebone.Controllers
{
    public class BareboneController : ControllerBase
    {
        public BareboneController(IStorage storage)
            : base(storage)
        {
        }

        public ActionResult Index()
        {
            return View(new IndexViewModelFactory().Create());
        }
    }
}