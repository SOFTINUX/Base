using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;

namespace Barebone.Controllers
{
    public abstract class ControllerBase : Controller, IRequestHandler
    {

        public ControllerBase()
        {

        }

        protected RedirectResult CreateRedirectToSelfResult()
        {
            return this.Redirect(this.Request.Path.Value + this.Request.QueryString.Value);
        }
    }
}