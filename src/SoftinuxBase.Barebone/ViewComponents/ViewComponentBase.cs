// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Barebone.ViewComponents
{
    public abstract class ViewComponentBase : ViewComponent
    {
        protected readonly IStorage _storage;

        protected readonly ILoggerFactory _loggerFactory;

        protected ViewComponentBase(IStorage storage_, ILoggerFactory loggerFactory_)
        {
            _storage = storage_;
            _loggerFactory = loggerFactory_;
        }

        protected ViewComponentBase(ILoggerFactory loggerFactory_)
        {
            _loggerFactory = loggerFactory_;
        }
    }
}