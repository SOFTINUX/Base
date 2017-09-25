using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;
using Security;

namespace Barebone.Controllers
{
    public class BareboneController : ControllerBase
    {
        public BareboneController(IStorage storage_)

        {
            Storage = storage_;
        }

        public ActionResult Index()
        {
            DatabaseInitializer.CheckAndInitialize(this);
            return View(new IndexViewModelFactory().Create());
        }
    }
}