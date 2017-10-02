using System;
using System.Linq;
using System.Reflection;
using ExtCore.Data.EntityFramework;
using Infrastructure;
using Security.Data.Entities;
using Security.Data.EntityFramework;

namespace Security
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private string _securityAssemblyName = Assembly.GetExecutingAssembly().FullName;

        /// <summary>
        /// Performs database base data inserts if no data
        /// </summary>
        /// <param name="context_"></param>
        public void CheckAndInitialize(IRequestHandler context_)
        {
            // 1. credential type
            bool mustInit = CheckInsertCredentialType(context_);

            if (!mustInit)
                return;

            // 2. user
            InsertUser(context_);

            // 3. credential
            InsertCredential(context_);

            // 4. permission level
            InsertPermissionLevel(context_);

            // 5. role
            InsertRole(context_);

            // 6. group (none)

            // 7. user-role
            InsertUserRole(context_);

            // 8. group-user (none)

            // 9. permission

            // 10. user-permission (none)

            // 11. role-permission

            // 12. group-permission (none)

            context_.Storage.Save();
            
        }

        private void InsertPermission(IRequestHandler context_)
        {
            PermissionRepository repo = context_.Storage.GetRepository<PermissionRepository>();
            // TODO use enum repo.Create(new Permission { }

            //  permissions d'administration défini dans Base permettent l'administration des users ("edit-user"), roles ("edit-role"), 
            // groupes ("edit-group"), permissions ("edit-permission"). Ils sont associés aux rôles administrator-owner et administrator avec le droit d'accès lecture-écriture.
        }

        private void InsertUserRole(IRequestHandler context_)
        {
            UserRoleRepository repo = context_.Storage.GetRepository<UserRoleRepository>();
            // super-admin
            repo.Create(new UserRole {RoleId = (int) Enums.SecurityRoleId.AdministratorOwner, UserId = 1});
            // admin
            repo.Create(new UserRole { RoleId = (int)Enums.SecurityRoleId.Administrator, UserId = 2 });
            // user
            repo.Create(new UserRole { RoleId = (int)Enums.SecurityRoleId.User, UserId = 3 });

        }

        private void InsertRole(IRequestHandler context_)
        {
            RoleRepository repo = context_.Storage.GetRepository<RoleRepository>();

            repo.Create(new Role { Code = "administrator-owner", Label = "Administrator Owner", OriginExtension = _securityAssemblyName });
            repo.Create(new Role { Code = "administrator", Label = "Administrator", OriginExtension = _securityAssemblyName });
            repo.Create(new Role { Code = "user", Label = "User", OriginExtension = _securityAssemblyName });
        }

        private void InsertPermissionLevel(IRequestHandler context_)
        {
            PermissionLevelRepository repo = context_.Storage.GetRepository<PermissionLevelRepository>();

            repo.Create(new PermissionLevel{Id = (int) Enums.PermissionLevelId.Never, Value = 1, Label = "Never", Tip = "No right, unmodifiable through right inheritance"});
            repo.Create(new PermissionLevel{Id = (int)Enums.PermissionLevelId.No, Value = 2, Label = "No", Tip = "No right, but could be allowed through right inheritance"});
            repo.Create(new PermissionLevel{Id = (int)Enums.PermissionLevelId.ReadOnly, Value = 4, Label = "Read-only", Tip = "Read-only access"});
            repo.Create(new PermissionLevel{Id = (int)Enums.PermissionLevelId.ReadWrite, Value = 8, Label = "Read-write", Tip = "Read-write access"});
        }

        private void InsertCredential(IRequestHandler context_)
        {
            CredentialRepository repo = context_.Storage.GetRepository<CredentialRepository>();
            
            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 1,
                Identifier = "superadmin",
                Secret = "123password"
            });

            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 2,
                Identifier = "admin",
                Secret = "123password"
            });

            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 3,
                Identifier = "user",
                Secret = "123password"
            });
        }

        private void InsertUser(IRequestHandler context_)
        {
            UserRepository repo = context_.Storage.GetRepository<UserRepository>();
            repo.Create(new User { DisplayName = "Super Administrator", FirstName = "Super", LastName = "Admin" });
            repo.Create(new User { DisplayName = "User", FirstName = "Test", LastName = "User" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context_"></param>
        /// <returns>False if database is already populated</returns>
        private bool CheckInsertCredentialType(IRequestHandler context_)
        {
            CredentialTypeRepository repo = context_.Storage.GetRepository<CredentialTypeRepository>();

            if (repo.All().Any())
                return false;

            CredentialType entity = new CredentialType {Code = "email", Label = "E-mail and password"};
            repo.Create(entity);
            return true;
        }
    }
}
 