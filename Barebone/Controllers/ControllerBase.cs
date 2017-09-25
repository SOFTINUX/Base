using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;
using Infrastructure;

namespace Barebone.Controllers
{
    public abstract class ControllerBase : Controller, IRequestHandler
    {

        public IStorage Storage { get; set; }

        protected ControllerBase(IStorage storage_)
        {
            Storage = storage_;
        }

        protected RedirectResult CreateRedirectToSelfResult()
        {
            return this.Redirect(this.Request.Path.Value + this.Request.QueryString.Value);
        }
    }
}