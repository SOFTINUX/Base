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
    public abstract class ControllerBase : Controller
    {
        /// <summary>
        /// Logger factory interface provided by services container.
        /// </summary>
        protected readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Storage interface provided by services container.
        /// </summary>
        protected readonly IStorage _storage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBase"/> class.
        /// </summary>
        /// <param name="storage_">Storage interface provided by services container.</param>
        /// <param name="loggerFactory_">Logger factory interface optionally provided by services container.</param>
        protected ControllerBase(IStorage storage_, ILoggerFactory loggerFactory_ = null)
        {
            _storage = storage_;
            _loggerFactory = loggerFactory_;
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