// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Collections.Generic;
using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Represents a credential type. The credential types are used to specify which identity providers
    /// (email and password, Microsoft, Google, Facebook and so on) are supported by the web application.
    /// Web application uses the different mechanisms to verify the credentials of the different credential types.
    /// </summary>
    public class CredentialType : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }

        /// <summary>
        /// Full name of extension's assembly, to manage data by extension (add, reset, remove).
        /// </summary>
        public string OriginExtension { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
