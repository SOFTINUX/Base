using ExtCore.Data.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Barebone.ViewComponents
{
    public abstract class ViewComponentBase : ViewComponent, IRequestHandler
    {
        public IStorage Storage { get; }

        protected ViewComponentBase(IStorage storage_)
        {
            Storage = storage_;
        }
    }
}