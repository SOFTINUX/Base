// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Reflection;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _securityAssemblyName = Assembly.GetExecutingAssembly().FullName;

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

            context_.Storage.Save();

            // 3. credential
            InsertCredential(context_);

            // 4. permission level
            InsertPermissionLevel(context_);

            // 5. role
            InsertRole(context_);

            // 6. group (none)

            // 7. permission
            InsertPermission(context_);

            context_.Storage.Save();

            // 8. user-role
            InsertUserRole(context_);

            // 9. group-user (none)

            // 10. user-permission (none)

            // 11. role-permission
            InsertRolePermission(context_);

            // 12. group-permission (none)

            context_.Storage.Save();

        }

        private void InsertRolePermission(IRequestHandler context_)
        {
            IRolePermissionRepository repo = context_.Storage.GetRepository<IRolePermissionRepository>();

            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditGroup, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditRole, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditPermission, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditGroup, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditRole, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditPermission, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)Enums.RoleId.User, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadOnly });
        }

        private void InsertPermission(IRequestHandler context_)
        {
            IPermissionRepository repo = context_.Storage.GetRepository<IPermissionRepository>();
            repo.Create(new Permission
            {
                Id = (int)Enums.Permission.PermissionId.EditUser,
                AdministratorOwner = true,
                Code = Enums.Permission.PERM_CODE_EDIT_USER,
                Label = "Edit users",
                OriginExtension = _securityAssemblyName
            });

            repo.Create(new Permission
            {
                Id = (int)Enums.Permission.PermissionId.EditRole,
                AdministratorOwner = true,
                Code = Enums.Permission.PERM_CODE_EDIT_ROLE,
                Label = "Edit roles",
                OriginExtension = _securityAssemblyName
            });

            repo.Create(new Permission
            {
                Id = (int)Enums.Permission.PermissionId.EditGroup,
                AdministratorOwner = true,
                Code = Enums.Permission.PERM_CODE_EDIT_GROUP,
                Label = "Edit groups",
                OriginExtension = _securityAssemblyName
            });

            repo.Create(new Permission
            {
                Id = (int)Enums.Permission.PermissionId.EditPermission,
                AdministratorOwner = true,
                Code = Enums.Permission.PERM_CODE_EDIT_PERMISSION,
                Label = "Edit permissions",
                OriginExtension = _securityAssemblyName
            });
        }

        private void InsertUserRole(IRequestHandler context_)
        {
            IUserRoleRepository repo = context_.Storage.GetRepository<IUserRoleRepository>();
            // super-admin
            repo.Create(new UserRole { RoleId = (int)Enums.RoleId.AdministratorOwner, UserId = 1 });
            // admin
            repo.Create(new UserRole { RoleId = (int)Enums.RoleId.Administrator, UserId = 2 });
            // user
            repo.Create(new UserRole { RoleId = (int)Enums.RoleId.User, UserId = 3 });

        }

        private void InsertRole(IRequestHandler context_)
        {
            IRoleRepository repo = context_.Storage.GetRepository<IRoleRepository>();

            repo.Create(new Role { Code = "administrator-owner", Label = "Administrator Owner", OriginExtension = _securityAssemblyName });
            repo.Create(new Role { Code = "administrator", Label = "Administrator", OriginExtension = _securityAssemblyName });
            repo.Create(new Role { Code = "user", Label = "User", OriginExtension = _securityAssemblyName });
        }

        private void InsertPermissionLevel(IRequestHandler context_)
        {
            IPermissionLevelRepository repo = context_.Storage.GetRepository<IPermissionLevelRepository>();

            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdNever, Value = (int)Enums.Permission.PermissionLevelValue.Never, Label = "Never", Tip = "No right, unmodifiable through right inheritance" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdNo, Value = (int)Enums.Permission.PermissionLevelValue.No, Label = "No", Tip = "No right, but could be allowed through right inheritance" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdReadOnly, Value = (int)Enums.Permission.PermissionLevelValue.ReadOnly, Label = "Read-only", Tip = "Read-only access" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdReadWrite, Value = (int)Enums.Permission.PermissionLevelValue.ReadWrite, Label = "Read-write", Tip = "Read-write access" });
        }

        private void InsertCredential(IRequestHandler context_)
        {
            ICredentialRepository repo = context_.Storage.GetRepository<ICredentialRepository>();
            string hashedPassword = new PasswordHasher<User>().HashPassword(null, "123password");

            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 1,
                Identifier = "adminowner",
                Secret = hashedPassword
            });

            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 2,
                Identifier = "admin",
                Secret = hashedPassword
            });

            repo.Create(new Credential
            {
                CredentialTypeId = 1,
                UserId = 3,
                Identifier = "user",
                Secret = hashedPassword
            });
        }

        private void InsertUser(IRequestHandler context_)
        {
            IUserRepository repo = context_.Storage.GetRepository<IUserRepository>();
            repo.Create(new User { DisplayName = "Super Administrator", FirstName = "Super", LastName = "Admin" });
            repo.Create(new User { DisplayName = "Administrator", FirstName = "Test", LastName = "Admin" });
            repo.Create(new User { DisplayName = "User", FirstName = "Test", LastName = "User" });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context_"></param>
        /// <returns>False if database is already populated</returns>
        private bool CheckInsertCredentialType(IRequestHandler context_)
        {
            ICredentialTypeRepository repo = context_.Storage.GetRepository<ICredentialTypeRepository>();

            if (repo.All().Any())
                return false;

            CredentialType entity = new CredentialType { Code = "email", Label = "E-mail and password" };
            repo.Create(entity);
            return true;
        }
    }
}
