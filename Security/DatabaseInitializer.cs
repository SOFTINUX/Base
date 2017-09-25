using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public static class DatabaseInitializer
    {
        public static void CheckAndInitialize(IRequestHandler context_)
        {
            IPermissionLevelRepository repo = context_.Storage.GetRepository<IPermissionLevelRepository>();
            IEnumerable<PermissionLevel> permissionLevels = repo.All();

            if (permissionLevels.Count() > 0)
                return;


        }
    }
}
