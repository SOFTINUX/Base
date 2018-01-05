// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    /// <summary>
    /// Abstract controller parent of all controllers, that enforces IStorage dependency injection and authenticated access
    /// </summary>
    [Authorize]
    public abstract class ControllerBase : Controller, IRequestHandler
    {

        public IStorage Storage { get; set; }

        protected ILoggerFactory _loggerFactory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container</param>
        /// <param name="loggerFactory_">Logger factory interface provided by services container, optionally</param>
        protected ControllerBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            Storage = storage_;
            _loggerFactory = loggerFactory_;
        }

        /// <summary>
        /// Redirects to same url but looses model.
        /// </summary>
        /// <returns></returns>
        protected RedirectResult CreateRedirectToSelfResult()
        {
            return Redirect(Request.Path.Value + Request.QueryString.Value);
        }
    }
}