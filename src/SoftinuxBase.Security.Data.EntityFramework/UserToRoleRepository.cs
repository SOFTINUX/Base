using System;
using System.Linq;
using ExtCore.Data.EntityFramework;
using SoftinuxBase.Security.Data.Abstractions;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;

namespace SoftinuxBase.Security.Data.EntityFramework
{
    public class UserToRoleRepository : RepositoryBase<UserToRole>, IUserToRoleRepository
    {
        public bool AddUserToRole(string userId_, string roleName_)
        {
            if (userId_ == null) throw new ArgumentNullException(nameof(userId_));
            if (roleName_ == null) throw new ArgumentNullException(nameof(roleName_));
            var userToRole = Find(userId_, roleName_);
            if (userToRole != null)
            {
                // TODO add log ($"The user already has the Role '{roleName_}'.");
                return false;
            }

            var roleToAdd = storageContext.Set<RoleToPermissions>().FirstOrDefault(roleToPermission_ => roleToPermission_.RoleName == roleName_);
            if (roleToAdd == null)
            {
                // TODO add log ($"I could not find the Role '{roleName_}'.");
                return false;
            }

            dbSet.Add(new UserToRole(userId_, roleToAdd));
            return true;
        }

        public UserToRole Find(string userId_, string roleName_)
        {
            return dbSet.FirstOrDefault(userToRole_ => userToRole_.UserId == userId_ && userToRole_.RoleName == roleName_);
        }
    }
}
