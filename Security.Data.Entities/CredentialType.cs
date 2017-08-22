// Licensed under the Apache License, Version 2.0.
using System.Collections.Generic;
using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Represents a credential type. The credential types are used to specify which identity providers
    /// (email and password, Microsoft, Google, Facebook and so on) are supported by the web application.
    /// Web application uses the different mechanisms to verify the credentials of the different credential types.
    /// Copied from Platformus.
    /// </summary>
    public class CredentialType : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }

        /// <summary>
        /// Referenced entities.
        /// </summary>
        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
