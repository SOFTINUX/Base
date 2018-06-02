using System.Linq;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;

namespace BaseTest.Common
{
    public static class DataSetupTools
    {
        /// <summary>
        /// Delete links:
        /// - betweeen roles and permissions
        /// - between users and permissions
        /// - between roles and users
        /// </summary>
        public static void DeleteAllLinks(IStorage storage_, UserManager<Security.Data.Entities.User> userManager_, RoleManager<IdentityRole<string>> roleManager_)
        {
            // Delete role-permission and user-permission links using repositories
            storage_.GetRepository<IRolePermissionRepository>().DeleteAll();
            storage_.GetRepository<IUserPermissionRepository>().DeleteAll();

            // Delete user-role permission using Identity managers
            var allUsers = userManager_.Users.ToList();
            var allRolesNames = roleManager_.Roles.Select(r => r.Name);
            foreach(var user in allUsers)
                userManager_.RemoveFromRolesAsync(user, allRolesNames);
        }
    }
}