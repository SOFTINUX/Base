using Extension2.ViewModels.Extension2;
using Microsoft.AspNetCore.Mvc;

namespace Extension2.Controllers
{
    public class Extension2Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }
    }
}