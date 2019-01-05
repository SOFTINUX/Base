// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Barebone.ViewModels
{
    public class ViewModelFactoryBase
    {
        protected IRequestHandler RequestHandler { get; set; }

        protected ILogger Logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestHandler_"></param>
        /// <param name="loggerFactory_">Optional</param>
        public ViewModelFactoryBase(IRequestHandler requestHandler_, ILoggerFactory loggerFactory_ = null)
        {
            RequestHandler = requestHandler_;
            Logger = loggerFactory_?.CreateLogger(GetType().FullName);
        }
    }
}