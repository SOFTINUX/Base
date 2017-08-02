using Microsoft.AspNetCore.Mvc;
using Extension1.ViewModels.Extension1;

namespace Extension1.Controllers
{
    public class Extension1Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }
    }
}