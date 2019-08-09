// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Barebone.ViewComponents
{
    public abstract class ViewComponentBase : ViewComponent
    {
#pragma warning disable SA1401 // FieldsMustBePrivate

        /// <summary>
        /// Storage interface provided by services container.
        /// </summary>
        protected readonly IStorage Storage;

        /// <summary>
        /// Logger factory interface provided by services container.
        /// </summary>
        protected readonly ILoggerFactory LoggerFactory;

#pragma warning restore SA1401 // FieldsMustBePrivate

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