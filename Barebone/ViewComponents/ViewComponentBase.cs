using ExtCore.Data.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Barebone.ViewComponents
{
    public abstract class ViewComponentBase : ViewComponent, IRequestHandler
    {
        public IStorage Storage { get; }
        
        public ILoggerFactory LoggerFactory { get; }

        protected ViewComponentBase(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            Storage = storage_;
            LoggerFactory = loggerFactory_;
        }

        protected ViewComponentBase(ILoggerFactory loggerFactory_)
        {
            LoggerFactory = loggerFactory_;
        }
    }
}