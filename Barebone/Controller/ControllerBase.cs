using Microsoft.AspNetCore.Mvc;
using ExtCore.Data.Abstractions;

namespace Barebone.Controllers
{
    public abstract class ControllerBase : Controller, IRequestHandler
    {
        public IStorage Storage { get; private set; }

        public ControllerBase(IStorage storage)
        {
            this.Storage = storage;
        }

        protected RedirectResult CreateRedirectToSelfResult()
        {
            return this.Redirect(this.Request.Path.Value + this.Request.QueryString.Value);
        }
    }
}