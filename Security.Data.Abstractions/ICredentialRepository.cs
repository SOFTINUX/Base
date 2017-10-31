using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface ICredentialRepository : IRepository
    {
        Credential WithKey(int entityId_);
        Credential WithKeys(int credentialTypeId_, string identifier_);
        IEnumerable<Credential> All();
        void Create(Credential entity_);
        void Edit(Credential entity_);
        void Delete(int entityId_);

    }
}
