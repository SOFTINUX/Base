using Microsoft.AspNetCore.Mvc;
using Extension1.ViewModels.Extension1;
using Microsoft.AspNetCore.Authorization;

namespace Extension1.Controllers
{
    public class Extension1Controller : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexViexModelFactory().Create());
        }

        //[Authorize("Admin | Extension1, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null || RW")]
        public ActionResult Admin()
        {
            return View();
        }
    }
}