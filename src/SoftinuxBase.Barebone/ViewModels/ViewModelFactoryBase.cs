// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Barebone.ViewModels
{
    public class ViewModelFactoryBase
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryBase"/> class.
        /// </summary>
        /// <param name="storage_">The data storage instance.</param>
        /// <param name="loggerFactory_">Optional.</param>
        public ViewModelFactoryBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            Storage = storage_;
            LoggerFactory = loggerFactory_;
        }
    }
}