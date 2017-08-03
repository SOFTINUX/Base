using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Barebone.ViewComponents
{
    public class ViewComponentBase : ViewComponent, IRequestHandler
    {
        public IStorage Storage { get; private set; }

        public ViewComponentBase(IStorage storage)
        {
            this.Storage = storage;
        }
    }
}