// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewComponents
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