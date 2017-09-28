﻿using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Data.Abstractions
{
    public interface IPermissionRepository : IRepository
    {
        Permission WithKey(int entityId_);
        IEnumerable<Permission> All();
        void Create(Permission entity_);
        void Edit(Permission entity_);
        void Delete(int entityId_);
    }
}