// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Logging;

namespace SoftinuxBase.Barebone.ViewModels
{
    public class ViewModelFactoryBase
    {
        protected readonly IStorage _storage;

        protected readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryBase"/> class.
        /// </summary>
        /// <param name="storage_"></param>
        /// <param name="loggerFactory_">Optional.</param>
        public ViewModelFactoryBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            _storage = storage_;
            _loggerFactory = loggerFactory_;
        }
    }
}