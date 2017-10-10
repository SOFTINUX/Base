using ExtCore.Data.Entities.Abstractions;

namespace Security.Data.Entities
{
    /// <summary>
    /// Represents a credential. The credentials are used to store the information that is used to sign in the users
    /// using the different identity providers (email and password, Microsoft, Google, Facebook and so on).
    /// The identity provider is specified by the credential type.
    /// Copied from Platformus.
    /// </summary>
    public class Credential : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CredentialTypeId { get; set; }
        public string Identifier { get; set; }
        public string Secret { get; set; }

        /// <summary>
        /// Referenced user entity.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Referenced credential type entity..
        /// </summary>
        public virtual CredentialType CredentialType { get; set; }
    }
}
