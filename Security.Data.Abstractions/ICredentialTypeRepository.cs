using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface ICredentialTypeRepository : IRepository
    {
        void Create(CredentialType entity_);
        CredentialType WithKey(int entityId_);
        IEnumerable<CredentialType> All();
        CredentialType WithCode(string code_);
    }
}
