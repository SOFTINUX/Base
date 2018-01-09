using System.Net;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Security.Common.Attributes;
using ControllerBase = Infrastructure.ControllerBase;

namespace Security.Controllers
{
    [Authorize("Administrator", "Security")]
    public class AdministrationController : ControllerBase
    {
        private ILogger _logger;

        public AdministrationController(IStorage storage_, ILoggerFactory loggerFactory_) : base(storage_, loggerFactory_)
        {
            _logger = _loggerFactory.CreateLogger(GetType().FullName);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Administration");
        }
    }
}