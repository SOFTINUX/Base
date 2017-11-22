using System;
using System.Collections.Generic;
using System.Linq;
using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Security.Data.Abstractions;
using Security.Data.Entities;

namespace Security.ServiceConfiguration
{
    // TODO restore ": IConfigureServicesAction" once unit test is ok (errors fixed)
    public class InitializeDatabase //: IConfigureServicesAction
    {
        /// <summary>
        /// Executes after ActivateAuthentication and ConfigureAuthentication service actions.
        /// </summary>
        public int Priority => 100;

        private IStorage _storage;

        public void Execute(IServiceCollection services_, IServiceProvider serviceProvider_)
        {
            _storage = serviceProvider_.GetService<IStorage>();

            IEnumerable<IExtensionDatabaseMetadata> extensionMetadatas =
                ExtensionManager.GetInstances<IExtensionDatabaseMetadata>().OrderBy(e_ => e_.Priority);

            // Recod all the entities that don't depend on another one then commit
            foreach (IExtensionDatabaseMetadata extensionMetadata in extensionMetadatas)
            {
                // Permissions
                RecordPermissions(extensionMetadata.PermissionCodeLabelAndFlags, GetAssemblyName(extensionMetadata));
                // Permissions level
                RecordPermissionsLevel(extensionMetadata.PermissionLevelIdValueLabelAndTips,  GetAssemblyName(extensionMetadata));
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

            // TODO FIXME find why despite the Save() call above, the next query that is insertion of
            // Credential that uses credential type id as FK generates a query failure in EF
            // line 140 of Security's ExtensionDatabaseMetadata.
            // The query uses a last insert row id that corresponds to nothing (a role!).
            // Even without a call to Save(). The problem seem to be the fact that we insert a FK value.


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
                if (repo.WithKeys(permission.Item1, permission.Item2) == null)
                {
                    repo.Create(new Permission { Code = permission.Item1, Label = permission.Item2, AdministratorOwner = permission.Item3, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

        private void RecordPermissionsLevel(IEnumerable<Tuple<int, byte, string, string>> permissionsLevel_, string extensionAssemblyName_)
        {
            if (permissionsLevel_ == null)
                return;
            IPermissionLevelRepository repo = _storage.GetRepository<IPermissionLevelRepository>();
            foreach (Tuple<int, byte, string, string> permissionLevel in permissionsLevel_)
            {
                if (repo.WithValue(permissionLevel.Item2) == null)
                {
                    repo.Create(new PermissionLevel { Id = permissionLevel.Item1,  Value = permissionLevel.Item2, Label = permissionLevel.Item3, Tip = permissionLevel.Item4 });
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
                if (repo.WithKeys(user.Item1, user.Item2, user.Item3) == null)
                {
                    repo.Create(new User { FirstName = user.Item1, LastName = user.Item2, DisplayName = user.Item3 });
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
                if (repo.WithCode(role.Key) == null)
                {
                    repo.Create(new CredentialType { Code = role.Key, Label = role.Value });
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
                if (repo.WithKeys(role.Key, role.Value) == null)
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
                if (repo.WithKeys(group.Key, group.Value) == null)
                {
                    repo.Create(new Group { Code = group.Key, Label = group.Value, OriginExtension = extensionAssemblyName_ });
                }
            }
        }

    }
}