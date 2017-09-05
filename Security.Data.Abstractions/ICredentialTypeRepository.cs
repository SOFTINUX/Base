using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface ICredentialTypeRepository : IRepository
    {
        CredentialType WithKey(int entityId_);
        IEnumerable<CredentialType> All();
    }
}
