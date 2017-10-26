using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    /// <summary>
    /// Abstract controller parent of all controllers, that enforces IStorage dependency injection
    /// </summary>
    public abstract class ControllerBase : Controller, IRequestHandler
    {

        public IStorage Storage { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container</param>
        /// <param name="loggerFactory_">Logger factory interface provided by services container, optionally</param>
        protected ControllerBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            Storage = storage_;
            LoggerFactory = loggerFactory_;
        }

        protected RedirectResult CreateRedirectToSelfResult()
        {
            return this.Redirect(this.Request.Path.Value + this.Request.QueryString.Value);
        }
    }
}