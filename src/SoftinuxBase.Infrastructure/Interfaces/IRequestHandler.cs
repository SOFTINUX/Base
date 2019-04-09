// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Http;

namespace SoftinuxBase.Infrastructure.Interfaces
{
    /// <summary>
    /// Interface to allow access to things we need to access from anywhere in application such as http context and storage context,
    /// and that are injected to controllers. This interface limits the acess of modules to only these controllers' properties.
    /// </summary>
    public interface IRequestHandler
    {
        /// <summary>
        /// Gets application HttpContext.
        /// </summary>
        HttpContext HttpContext { get; }

        /// <summary>
        /// Gets application storage.
        /// </summary>
        IStorage Storage { get; }
    }
}