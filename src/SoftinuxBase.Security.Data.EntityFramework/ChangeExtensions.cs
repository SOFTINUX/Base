#pragma warning disable SA1636
/* Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
 Licensed under MIT license. See License.txt in the project root for license information. */

using System.Linq;
using Microsoft.EntityFrameworkCore;
using SoftinuxBase.Security.Data.Entities;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public static class ChangeExtensions
    {
        public static bool UserPermissionsMayHaveChanged(this DbContext context_)
        {
            return context_.ChangeTracker.Entries()
                .Any(x => (x.Entity is IChangeEffectsUser && x.State == EntityState.Modified) ||
                          (x.Entity is IAddRemoveEffectsUser &&
                                (x.State == EntityState.Added || x.State == EntityState.Deleted)));
        }
    }
}