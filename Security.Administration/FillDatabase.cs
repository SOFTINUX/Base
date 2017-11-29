using System;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.Administration
{
   /// <summary>
   /// This class should be used for initial installation so that we install database data once database schema is ready.
   /// To be reworked later as an administration part.
   /// </summary>
    public class FillDatabase
    {
        /// <summary>
        /// Looks for all implementations of IExtensionDatabaseMetadata that allow the system to know which
        /// persistent data the extensions provide.
        /// Creates the entities that have no reference to other entities (permissions, roles...)
        /// then calls custom code to create data in links tables.
        /// The extensions provide the information about entities to save and custom code for saving to links tables.
        ///
        /// This action executes after ActivateAuthentication and ConfigureAuthentication service actions.
        /// </summary>
        public int Priority => 100;

        private IStorage _storage;

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            _storage = serviceProvider_.GetService<IStorage>();

            IEnumerable<IExtensionDatabaseMetadata> extensionMetadatas =
                ExtensionManager.GetInstances<IExtensionDatabaseMetadata>().OrderBy(e_ => e_.Priority);

            // Record all the entities that don't depend on another one then commit
            foreach (IExtensionDatabaseMetadata extensionMetadata in extensionMetadatas)
            {
                // Permissions
                RecordPermissions(extensionMetadata.PermissionCodeLabelAndFlags, GetAssemblyName(extensionMetadata));
                // Permissions level
                RecordPermissionsLevel(extensionMetadata.PermissionLevelValueLabelAndTips);
                // Roles
                RecordRoles(extensionMetadata.RoleCodeAndLabels, GetAssemblyName(extensionMetadata));
                // Groups
                RecordGroups(extensionMetadata.GroupCodeAndLabels, GetAssemblyName(extensionMetadata));
                // Credential Type
                RecordCredentialsType(extensionMetadata.CredentialTypeCodeAndLabels,  GetAssemblyName(extensionMetadata));
                // User
                RecordUsers(extensionMetadata.UserFirstnameLastnameAndDisplayNames,  GetAssemblyName(extensionMetadata));
            }

            _storage.Save();

            // Record the links between entities then commit
            foreach (IExtensionDatabaseMetadata extensionMetadata in extensionMetadatas)
            {
                // Additional links configuration
                extensionMetadata.ConfigureLinks(_storage);
            }
            _storage.Save();

        }

        private string GetAssemblyName(IExtensionDatabaseMetadata extension_)
        {
            return extension_.GetType().Assembly.GetName().Name;
        }

        private void RecordPermissions(IEnumerable<Tuple<string, string, bool>> permissions_, string extensionAssemblyName_)
        {
            if (permissions_ == null)
                return;
            IPermissionRepository repo = _storage.GetRepository<IPermissionRepository>();
            foreach (Tuple<string, string, bool> permission in permissions_)
            {
                if (repo.FindBy(permission.Item1, extensionAssemblyName_) == null)
                {
                    repo.Create(new Permission { Code = permission.Item1, Label = permission.Item2, AdministratorOwner = permission.Item3, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

        private void RecordPermissionsLevel(IEnumerable<Tuple<PermissionLevelValue, string, string>> permissionsLevel_)
        {
            if (permissionsLevel_ == null)
                return;
            IPermissionLevelRepository repo = _storage.GetRepository<IPermissionLevelRepository>();
            foreach (Tuple<PermissionLevelValue, string, string> permissionLevel in permissionsLevel_)
            {
                if (repo.FindBy(permissionLevel.Item1) == null)
                {
                    repo.Create(new PermissionLevel {  Value = (byte) permissionLevel.Item1, Label = permissionLevel.Item2, Tip = permissionLevel.Item3 });
                }
            }
        }

        private void RecordUsers(IEnumerable<Tuple<string, string, string>> user_, string extensionAssemblyName_)
        {
             if (user_ == null)
                return;
            IUserRepository repo = _storage.GetRepository<IUserRepository>();
            foreach (Tuple<string, string, string> user in user_)
            {
                if (repo.FindBy(user.Item1, user.Item2, user.Item3, extensionAssemblyName_) == null)
                {
                    repo.Create(new User { FirstName = user.Item1, LastName = user.Item2, DisplayName = user.Item3, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

        private void RecordCredentialsType(IEnumerable<KeyValuePair<string, string>> credentialsTypes_, string extensionAssemblyName_)
        {
            if (credentialsTypes_ == null)
                return;
            ICredentialTypeRepository repo = _storage.GetRepository<ICredentialTypeRepository>();
            foreach (KeyValuePair<string, string> role in credentialsTypes_)
            {
                if (repo.FindBy(role.Key, extensionAssemblyName_) == null)
                {
                    repo.Create(new CredentialType { Code = role.Key, Label = role.Value, OriginExtension = extensionAssemblyName_});
                }
            }
        }

        private void RecordRoles(IEnumerable<KeyValuePair<string, string>> roles_, string extensionAssemblyName_)
        {
            if (roles_ == null)
                return;
            IRoleRepository repo = _storage.GetRepository<IRoleRepository>();
            foreach (KeyValuePair<string, string> role in roles_)
            {
                if (repo.FindBy(role.Key, extensionAssemblyName_) == null)
                {
                    repo.Create(new Role { Code = role.Key, Label = role.Value, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

        private void RecordGroups(IEnumerable<KeyValuePair<string, string>> groups_, string extensionAssemblyName_)
        {
            if (groups_ == null)
                return;
            IGroupRepository repo = _storage.GetRepository<IGroupRepository>();
            foreach (KeyValuePair<string, string> group in groups_)
            {
                if (repo.FindBy(group.Key, extensionAssemblyName_) == null)
                {
                    repo.Create(new Group { Code = group.Key, Label = group.Value, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

    }
}