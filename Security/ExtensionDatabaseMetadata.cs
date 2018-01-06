// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Data.Abstractions;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security
{
    public class ExtensionDatabaseMetadata : IExtensionDatabaseMetadata
    {
        // this value will not change
        private const string _securityAssemblyName = "Security";

        private Tuple<string, string, string> _superAdminUserData =
            new Tuple<string, string, string>("Super", "Admin", "Super Administrator");

        private Tuple<string, string, string> _adminUserData =
            new Tuple<string, string, string>("Test", "Admin", "Administrator");

        private Tuple<string, string, string> _userUserData =
            new Tuple<string, string, string>("Test", "User", "User");

        private KeyValuePair<string, string> _credentialTypeData =
            new KeyValuePair<string, string>("email", "E-mail and password");

        private IStorage _storage;

        public uint Priority => 0;

        // used in several methods so declared here and gat valued in ConfigureLinks().
        //private IRoleRepository _roleRepo;
        //Role _adminOwnerRole, _adminRole, _userRole;


        ///// <summary>
        ///// The permissions related to this extension administration.
        ///// </summary>
        //public IEnumerable<Tuple<string, string, bool>> PermissionCodeLabelAndFlags =>
        //    new[]
        //    {
        //        new Tuple<string, string, bool>(Permissions.PERM_CODE_EDIT_PERMISSION, "Permissions management", true),
        //        new Tuple<string, string, bool>(Permissions.PERM_CODE_EDIT_ROLE, "Roles management", true),
        //        new Tuple<string, string, bool>(Permissions.PERM_CODE_EDIT_USER, "Users management", true),
        //        new Tuple<string, string, bool>(Permissions.PERM_CODE_EDIT_GROUP, "Groups management", true)
        //    };

        ///// <summary>
        ///// The base roles.
        ///// </summary>
        //public IEnumerable<KeyValuePair<string, string>> RoleCodeAndLabels =>
        //    new[]
        //    {
        //        new KeyValuePair<string, string>(Roles.ROLE_CODE_ADMINISTRATOR_OWNER, "Administrator Owner"),
        //        new KeyValuePair<string, string>(Roles.ROLE_CODE_ADMINISTRATOR, "Administrator"),
        //        new KeyValuePair<string, string>(Roles.ROLE_CODE_USER, "Administrator User"),
        //    };

        ///// <summary>
        ///// No groups by default.
        ///// </summary>
        //public IEnumerable<KeyValuePair<string, string>> GroupCodeAndLabels => null;

        ///// <summary>
        ///// The base credential types.
        ///// </summary>
        //public IEnumerable<KeyValuePair<string, string>> CredentialTypeCodeAndLabels =>
        //    new[]
        //    {
        //        _credentialTypeData
        //    };

        ///// <summary>
        ///// The permission levels used in Security extension.
        ///// </summary>
        //public IEnumerable<Tuple<PermissionLevelValue, string, string>> PermissionLevelValueLabelAndTips =>
        //    new[]
        //    {
        //        new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.Never, "Never",
        //            "No right, unmodifiable through right inheritance"),
        //        new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.No, "No",
        //            "No right, but could be allowed through right inheritance"),
        //        new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.ReadOnly, "Read-only", "Read-only access"),
        //        new Tuple<PermissionLevelValue, string, string>(PermissionLevelValue.ReadWrite, "Read-write", "Read-write access"),
        //    };

        //public IEnumerable<Tuple<string, string, string>> UserFirstnameLastnameAndDisplayNames =>
        //    new[]
        //    {
        //        _superAdminUserData,
        //        _adminUserData,
        //        _userUserData
        //    };

        public void ConfigureLinks(IStorage storage_)
        {
        //    _storage = storage_;

        //    // Get the role entities we need
        //    _roleRepo = _storage.GetRepository<IRoleRepository>();
        //    _adminOwnerRole = _roleRepo.FindBy(Roles.ROLE_CODE_ADMINISTRATOR_OWNER, _securityAssemblyName);
        //    _adminRole = _roleRepo.FindBy(Roles.ROLE_CODE_ADMINISTRATOR, _securityAssemblyName);
        //    _userRole = _roleRepo.FindBy(Roles.ROLE_CODE_USER, _securityAssemblyName);

        //    // 1. credential
        //    InsertCredential();

        //    // 2. user-role
        //    InsertUserRole();

        //    // 3. group-user (none)

        //    // 4. user-permission (none)

        //    // 5. role-permission
        //    InsertRolePermission();

        //    // 6. group-permission (none)
        }

        //private void InsertRolePermission()
        //{
        //    IRolePermissionRepository repo = _storage.GetRepository<IRolePermissionRepository>();

        //    // Get the permission level entities we need
        //    IPermissionLevelRepository levelRepo = _storage.GetRepository<IPermissionLevelRepository>();
        //    PermissionLevel readLevel = levelRepo.FindBy(PermissionLevelValue.ReadOnly);
        //    PermissionLevel writeLevel = levelRepo.FindBy(PermissionLevelValue.ReadWrite);

        //    // Get the permission entities we need
        //    IPermissionRepository permRepo = _storage.GetRepository<IPermissionRepository>();
        //    Permission editGroupPerm = permRepo.FindBy(Permissions.PERM_CODE_EDIT_GROUP, _securityAssemblyName);
        //    Permission editUserPerm = permRepo.FindBy(Permissions.PERM_CODE_EDIT_USER, _securityAssemblyName);
        //    Permission editRolePerm = permRepo.FindBy(Permissions.PERM_CODE_EDIT_ROLE, _securityAssemblyName);
        //    Permission editPermissionPerm = permRepo.FindBy(Permissions.PERM_CODE_EDIT_PERMISSION, _securityAssemblyName);

        //    // Create the links
        //    repo.Create(new RolePermission { RoleId = _adminOwnerRole.Id, PermissionId = editGroupPerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminOwnerRole.Id, PermissionId = editUserPerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminOwnerRole.Id, PermissionId = editRolePerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminOwnerRole.Id, PermissionId = editPermissionPerm.Id, PermissionLevelId = writeLevel.Id });

        //    repo.Create(new RolePermission { RoleId = _adminRole.Id, PermissionId = editGroupPerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminRole.Id, PermissionId = editUserPerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminRole.Id, PermissionId = editRolePerm.Id, PermissionLevelId = writeLevel.Id });
        //    repo.Create(new RolePermission { RoleId = _adminRole.Id, PermissionId = editPermissionPerm.Id, PermissionLevelId = writeLevel.Id });

        //    repo.Create(new RolePermission { RoleId = _userRole.Id, PermissionId = editUserPerm.Id, PermissionLevelId = readLevel.Id });
        //}

        //private void InsertUserRole()
        //{
        //    IUserRoleRepository repo = _storage.GetRepository<IUserRoleRepository>();
        //    IUserRepository userRepo = _storage.GetRepository<IUserRepository>();
        //    // super-admin
        //    repo.Create(new UserRole { RoleId = _adminOwnerRole.Id, UserId = userRepo.FindBy(_superAdminUserData.Item1, _superAdminUserData.Item2, _superAdminUserData.Item3, _securityAssemblyName).Id });
        //    // admin
        //    repo.Create(new UserRole { RoleId = _adminRole.Id, UserId = userRepo.FindBy(_adminUserData.Item1, _adminUserData.Item2, _adminUserData.Item3, _securityAssemblyName).Id });
        //    // user
        //    repo.Create(new UserRole { RoleId = _userRole.Id, UserId = userRepo.FindBy(_userUserData.Item1, _userUserData.Item2, _userUserData.Item3, _securityAssemblyName).Id });

        //}

        //private void InsertCredential()
        //{
        //    ICredentialRepository repo = _storage.GetRepository<ICredentialRepository>();
        //    IUserRepository userRepo = _storage.GetRepository<IUserRepository>();
        //    ICredentialTypeRepository credTypeRepo = _storage.GetRepository<ICredentialTypeRepository>();

        //    string hashedPassword = new PasswordHasher<User>().HashPassword(null, "123password");
        //    int credentialTypeId = credTypeRepo.FindBy(_credentialTypeData.Key, _securityAssemblyName).Id;
        //    repo.Create(new Credential
        //    {
        //        CredentialTypeId = credentialTypeId,
        //        UserId = userRepo.FindBy(_superAdminUserData.Item1, _superAdminUserData.Item2, _superAdminUserData.Item3, _securityAssemblyName).Id,
        //        Identifier = "adminowner",
        //        Secret = hashedPassword
        //    });

        //    repo.Create(new Credential
        //    {
        //        CredentialTypeId = credentialTypeId,
        //        UserId = userRepo.FindBy(_adminUserData.Item1, _adminUserData.Item2, _adminUserData.Item3, _securityAssemblyName).Id,
        //        Identifier = "admin",
        //        Secret = hashedPassword
        //    });

        //    repo.Create(new Credential
        //    {
        //        CredentialTypeId = credentialTypeId,
        //        UserId = userRepo.FindBy(_userUserData.Item1, _userUserData.Item2, _userUserData.Item3, _securityAssemblyName).Id,
        //        Identifier = "user",
        //        Secret = hashedPassword
        //    });
        //}
    }
}
