// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace Barebone.ViewModels
{
    public class ViewModelFactoryBase
    {
        protected IRequestHandler RequestHandler { get; set; }

        protected ILogger _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="requestHandler_"></param>
        /// <param name="loggerFactory_">Optional</param>
        public ViewModelFactoryBase(IRequestHandler requestHandler_, ILoggerFactory loggerFactory_ = null)
        {
            RequestHandler = requestHandler_;
            _logger = loggerFactory_?.CreateLogger(GetType().FullName);
        }
    }
}