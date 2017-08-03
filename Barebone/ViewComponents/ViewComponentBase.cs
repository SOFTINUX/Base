using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Barebone.ViewComponents
{
    public abstract class ViewComponentBase : ViewComponent, Barebone.IRequestHandler
    {
        //public IStorage Storage { get; private set; }

        //public ViewComponentBase(IStorage storage)
        public ViewComponentBase()
        {
            //this.Storage = storage;
        }
    }
}