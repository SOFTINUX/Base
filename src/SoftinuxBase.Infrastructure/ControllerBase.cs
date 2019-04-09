// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Infrastructure
{
    /// <summary>
    /// Abstract controller parent of all controllers, that enforces IStorage dependency injection and authenticated access.
    /// </summary>
    [Authorize]
    public abstract class ControllerBase : Controller, IRequestHandler
    {
        /// <summary>
        /// Logger Factory instance. Can't be in private member.
        /// </summary>
        protected readonly ILoggerFactory LoggerFactory;

        // TODO make protected readonly ?
        public IStorage Storage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase"/> class.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="loggerFactory_">Logger factory interface provided by services container, optionally.</param>
        protected ControllerBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            Storage = storage_;
            LoggerFactory = loggerFactory_;
        }

        /// <summary>
        /// Redirects to same url but looses model.
        /// </summary>
        /// <returns>Return url string.</returns>
        protected RedirectResult CreateRedirectToSelfResult()
        {
            return Redirect(Request.Path.Value + Request.QueryString.Value);
        }
    }
}