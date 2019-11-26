using ExtCore.Data.Abstractions;
using SoftinuxBase.Security.DataLayer.ExtraAuthClasses;

namespace SoftinuxBase.Security.Data.Abstractions
{
    public interface IUserToRoleRepository : IRepository
    {
        /// <summary>
        /// Link an user to a role. Does nothing if the link already exists.
        /// </summary>
        /// <param name="userId_">User Id.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>True when a record was created.</returns>
        bool AddUserToRole(string userId_, string roleName_);

        /// <summary>
        /// Find a record.
        /// </summary>
        /// <param name="userId_">User Id.</param>
        /// <param name="roleName_">Role name.</param>
        /// <returns>True when a record was found for this user and this role.</returns>
        UserToRole Find(string userId_, string roleName_);
    }
}
