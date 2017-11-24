// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
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
        private Tuple<string, string, string> _superAdminData =
            new Tuple<string, string, string>("Super", "Admin", "Super Administrator");

        private Tuple<string, string, string> _adminData =
            new Tuple<string, string, string>("Test", "Admin", "Test Admin");

        private Tuple<string, string, string> _userData =
            new Tuple<string, string, string>("Test", "User", "Test User");

        private KeyValuePair<string, string> _credentialTypeData =
            new KeyValuePair<string, string>("email", "E-mail and password");

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
                _credentialTypeData
            };

        /// <summary>
        /// The permission levels used in Security extension.
        /// </summary>
        public IEnumerable<Tuple<PermissionLevelValue, string, string>> PermissionLevelValueLabelAndTips =>
            new[]
            {
                new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.Never, "Never",
                    "No right, unmodifiable through right inheritance"),
                new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.No, "No",
                    "No right, but could be allowed through right inheritance"),
                new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.ReadOnly, "Read-only", "Read-only access"),
                new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.ReadWrite, "Read-write", "Read-write access"),
            };

        public IEnumerable<Tuple<string, string, string>> UserFirstnameLastnameAndDisplayNames =>
            new[]
            {
                _superAdminData,
                _adminData,
                _userData
            };

        public void ConfigureLinks(IStorage storage_)
        {
            _storage = storage_;

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

            // Get the permission level we need
            IPermissionLevelRepository levelRepo = _storage.GetRepository<IPermissionLevelRepository>();
            PermissionLevel readLevel = levelRepo.ByValue(PermissionLevelValue.ReadOnly);
            PermissionLevel writeLevel = levelRepo.ByValue(PermissionLevelValue.ReadWrite);

            // Create the links
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditGroup, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditRole, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.AdministratorOwner, PermissionId = (int)Permission.PermissionId.EditPermission, PermissionLevelId = writeLevel.Id });

            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditGroup, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditRole, PermissionLevelId = writeLevel.Id });
            repo.Create(new RolePermission { RoleId = (int)RoleId.Administrator, PermissionId = (int)Permission.PermissionId.EditPermission, PermissionLevelId = writeLevel.Id });

            repo.Create(new RolePermission { RoleId = (int)RoleId.User, PermissionId = (int)Permission.PermissionId.EditUser, PermissionLevelId = readLevel.Id });
        }

        private void InsertUserRole()
        {
            IUserRoleRepository repo = _storage.GetRepository<IUserRoleRepository>();
            IUserRepository userRepo = _storage.GetRepository<IUserRepository>();
            // super-admin
            repo.Create(new UserRole { RoleId = (int)RoleId.AdministratorOwner, UserId = userRepo.WithKeys(_superAdminData.Item1, _superAdminData.Item2, _superAdminData.Item3).Id });
            // admin
            repo.Create(new UserRole { RoleId = (int)RoleId.Administrator, UserId = userRepo.WithKeys(_adminData.Item1, _adminData.Item2, _adminData.Item3).Id });
            // user
            repo.Create(new UserRole { RoleId = (int)RoleId.User, UserId = userRepo.WithKeys(_userData.Item1, _userData.Item2, _userData.Item3).Id });

        }

        private void InsertCredential()
        {
            ICredentialRepository repo = _storage.GetRepository<ICredentialRepository>();
            IUserRepository userRepo = _storage.GetRepository<IUserRepository>();
            ICredentialTypeRepository credTypeRepo = _storage.GetRepository<ICredentialTypeRepository>();

            string hashedPassword = new PasswordHasher<User>().HashPassword(null, "123password");
            int credentialTypeId = credTypeRepo.WithCode(_credentialTypeData.Key).Id;
            repo.Create(new Credential
            {
                CredentialTypeId = credentialTypeId,
                UserId = userRepo.WithKeys(_superAdminData.Item1, _superAdminData.Item2, _superAdminData.Item3).Id,
                Identifier = "adminowner",
                Secret = hashedPassword
            });

            repo.Create(new Credential
            {
                CredentialTypeId = credentialTypeId,
                UserId = userRepo.WithKeys(_adminData.Item1, _adminData.Item2, _adminData.Item3).Id,
                Identifier = "admin",
                Secret = hashedPassword
            });

            repo.Create(new Credential
            {
                CredentialTypeId = credentialTypeId,
                UserId = userRepo.WithKeys(_userData.Item1, _userData.Item2, _userData.Item3).Id,
                Identifier = "user",
                Secret = hashedPassword
            });
        }
    }
}
