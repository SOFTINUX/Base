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

        /// <summary>
        /// Necessary public empty constructor.
        /// </summary>
        public InitializeDatabase() { }

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
                // Roles
                RecordRoles(extensionMetadata.RoleCodeAndLabels, GetAssemblyName(extensionMetadata));
                // Groups
                RecordGroups(extensionMetadata.GroupCodeAndLabels, GetAssemblyName(extensionMetadata));
                // TODO the other entities
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
                if (repo.WithKeys(permission.Item1, permission.Item2) == null)
                {
                    repo.Create(new Permission { Code = permission.Item1, Label = permission.Item2, AdministratorOwner = permission.Item3, OriginExtension = extensionAssemblyName_ });
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