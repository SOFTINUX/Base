// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Security.Common.Enums;
using Security.Data.Abstractions;
using Security.Data.Entities;
using Permission = Security.Enums.Permission;

namespace Security
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        private IStorage _storage;

        public uint Priority => 0;

        /// <summary>
        /// The permissions related to this extension administration.
        /// </summary>
        public IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
            new[]
            {
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_PERMISSION, "Permissions management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_ROLE, "Roles management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_USER, "Users management", true),
                new Tuple<string, string, bool>(Permission.PERM_CODE_EDIT_GROUP, "Groups management", true)
            };

        /// <summary>
        /// The base roles.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels =>
            new[]
            {
                new KeyValuePair<string, string>("administrator-owner", "Administrator Owner"),
                new KeyValuePair<string, string>("administrator", "Administrator"),
                new KeyValuePair<string, string>("user", "Administrator User"),
            };

        /// <summary>
        /// No groups by default.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;

        /// <summary>
        /// The base credential types.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> CredentialTypeCodeAndLabels =>
            new[]
            {
                new KeyValuePair<string, string>("email", "E-mail and password")
            };

        /// <summary>
        /// The permission levels used in Security extension.
        /// </summary>
        public IEnumerable<Tuple<int, int, string, string>> PermissionLevelIdValueLabelAndTips =>
            new[]
            {
                new Tuple<int, int, string, string>((int) Permission.PermissionLevelId.IdNever,
                    (int) Permission.PermissionLevelValue.Never, "Never",
                    "No right, unmodifiable through right inheritance"),
                new Tuple<int, int, string, string>((int) Permission.PermissionLevelId.IdNo,
                    (int) Permission.PermissionLevelValue.No, "No",
                    "No right, but could be allowed through right inheritance"),
                new Tuple<int, int, string, string>((int) Permission.PermissionLevelId.IdReadOnly,
                    (int) Permission.PermissionLevelValue.ReadOnly, "Read-only", "Read-only access"),
                new Tuple<int, int, string, string>((int) Permission.PermissionLevelId.IdReadWrite,
                    (int) Permission.PermissionLevelValue.ReadWrite, "Read-write", "Read-write access"),
            };

        public IEnumerable<Tuple<string, string, string>> UserFirstnameLastnameAndDisplayNames =>
            new[]
            {
                new Tuple<string, string, string>("Super", "Admin", "Super Administrator"),
                new Tuple<string, string, string>("Test", "Admin", "Test Admin"),
                new Tuple<string, string, string>("Test", "User", "Test User"),
            };

        public void ConfigureLinks(IStorage storage_)
        {
            _storage = storage_;

            // 1. credential
            // TODO uncomment  InsertCredential();

            // 2. user-role
            // TODO uncomment   InsertUserRole();

            // 3. group-user (none)

            // 4. user-permission (none)

            // 5. role-permission
            // TODO uncomment   InsertRolePermission();

            // 6. group-permission (none)
        }

        private void InsertRolePermission()
        {
            IRolePermissionRepository repo = _storage.GetRepository<IRolePermissionRepository>();

            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditGroup, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditRole, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditPermission, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditGroup, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditRole, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditPermission, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadWrite });

            repo.Create(new RolePermission { RoleId = (int)RoleId.User, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = (int)Permission.PermissionLevelId.IdReadOnly });
        }

        // TODO don't hardcode user ids but query them
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
        // TODO don't hardcode credential type ids/user ids but query them
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
    }
}
