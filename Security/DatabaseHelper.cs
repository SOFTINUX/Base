// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System.Linq;
using System.Reflection;
using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Identity;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    // TODO delete this class

    /// <summary>
    /// Helper that inserts additional entities (credential type, permission level, user) and dependent entities (credentil and links to permission, user, role).
    /// This is complementary to standard database initialization provided by ExtensionDatabaseMetadata.
    /// </summary>
    public class DatabaseHelper
    {
        private readonly IStorage _storage;

        private readonly string _securityAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public DatabaseHelper(IStorage storage_)
        {
            _storage = storage_;
        }

        public void CheckAndInitialize()
        {
            if (CheckThatMustInsert())
            {
                InsertEntities();
                _storage.Save();
                InsertDependentEntities();
                // don't save because the other extensions insert entities of same type, else EF tracking fails.
            }
        }

        private bool CheckThatMustInsert()
        {
            ICredentialTypeRepository repo = _storage.GetRepository<ICredentialTypeRepository>();

            return !repo.All().Any();

        }

        /// <summary>
        /// Insert entities that have no foreign keys. Commit needs to be done after all these inserts.
        /// </summary>
        private void InsertEntities()
        {
            // 1. credential type
            InsertCredentialType();

            // 2. user
            InsertUser();

            // 3. permission level
            InsertPermissionLevel();

        }

        /// <summary>
        /// Insert entities that have foreign keys. The entities that define the primary key should have been commited first.
        /// </summary>
        private void InsertDependentEntities()
        {
            // 1. credential
            InsertCredential();

            // 2. user-role
            InsertUserRole();

            // 3. group-user (none)

            // 4. user-permission (none)

            // 5. role-permission
            InsertRolePermission();

            // 6. group-permission (none)
        }

        private void InsertRolePermission()
        {
            IRolePermissionRepository repo = _storage.GetRepository<IRolePermissionRepository>();

            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditGroup, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditRole, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Enums.Permission.PermissionId.EditPermission, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditGroup, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditRole, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Enums.Permission.PermissionId.EditPermission, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)RoleId.User, PermissionId = (int)Enums.Permission.PermissionId.EditUser, PermissionLevelId = (int)Enums.Permission.PermissionLevelId.IdReadOnly });
        }

        private void InsertUserRole()
        {
            IUserRoleRepository repo = _storage.GetRepository<IUserRoleRepository>();
            // super-admin
            repo.Create(new UserRole { RoleId = (int)RoleId.AdministratorOwner, UserId = 1 });
            // admin
            repo.Create(new UserRole { RoleId = (int)RoleId.Administrator, UserId = 2 });
            // user
            repo.Create(new UserRole { RoleId = (int)RoleId.User, UserId = 3 });

        }

        private void InsertPermissionLevel()
        {
            IPermissionLevelRepository repo = _storage.GetRepository<IPermissionLevelRepository>();

            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdNever, Value = (int)Enums.Permission.PermissionLevelValue.Never, Label = "Never", Tip = "No right, unmodifiable through right inheritance" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdNo, Value = (int)Enums.Permission.PermissionLevelValue.No, Label = "No", Tip = "No right, but could be allowed through right inheritance" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdReadOnly, Value = (int)Enums.Permission.PermissionLevelValue.ReadOnly, Label = "Read-only", Tip = "Read-only access" });
            repo.Create(new PermissionLevel { Id = (int)Enums.Permission.PermissionLevelId.IdReadWrite, Value = (int)Enums.Permission.PermissionLevelValue.ReadWrite, Label = "Read-write", Tip = "Read-write access" });
        }

        private void InsertCredential()
        {
            ICredentialRepository repo = _storage.GetRepository<ICredentialRepository>();
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

        private void InsertUser()
        {
            IUserRepository repo = _storage.GetRepository<IUserRepository>();
            repo.Create(new User { DisplayName = "Super Administrator", FirstName = "Super", LastName = "Admin" });
            repo.Create(new User { DisplayName = "Administrator", FirstName = "Test", LastName = "Admin" });
            repo.Create(new User { DisplayName = "User", FirstName = "Test", LastName = "User" });
        }

        private void InsertCredentialType()
        {
            ICredentialTypeRepository repo = _storage.GetRepository<ICredentialTypeRepository>();
            repo.Create(new CredentialType { Code = "email", Label = "E-mail and password" });
        }
    }
}
