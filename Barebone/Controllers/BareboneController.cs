using Microsoft.AspNetCore.Mvc;
using Barebone.ViewModels.Barebone;
using ExtCore.Data.Abstractions;
using Infrastructure;
using ControllerBase = Infrastructure.ControllerBase;

namespace Barebone.Controllers
{
    public class BareboneController : ControllerBase
    {
        private readonly IDatabaseInitializer _databaseInitializer;

        public BareboneController(IStorage storage_, IDatabaseInitializer databaseInitializer_) : base(storage_)
        {
            _databaseInitializer = databaseInitializer_;
        }

        public ActionResult Index()
        {
            _databaseInitializer.CheckAndInitialize(this);
            return View(new IndexViewModelFactory().Create());
        }
    }
}