// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using SoftinuxBase.Security.Permissions;

namespace SoftinuxBase.Security.ViewModels.Permissions
{
    /// <summary>
    /// A <see cref="PermissionDisplay"/> with an additional property that allows to have two groups.
    /// </summary>
    public class SelectablePermissionDisplay : PermissionDisplay
    {
        /// <summary>
        /// Indicates that this PermissionDisplay is selected, in current context (for example the PermissionDisplay associated to a role).
        /// </summary>
        public bool Selected {get; set;}
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="section_">Permission section.</param>
        /// <param name="name_">Permission short name.</param>
        /// <param name="description_">Permission description.</param>
        /// <param name="extensionName_">Extension name.</param>
        /// <param name="permissionEnumValue_">Permission enum value.</param>
        /// <param name="permissionEnumType_">Permission enum Type.</param>
        /// <param name="moduleName_">Module name.</param>
        /// <param name="selected_">Selection indicator.</param>
        public SelectablePermissionDisplay(string section_, string name_, string description_, string extensionName_, short permissionEnumValue_,
            Type permissionEnumType_, string moduleName_, bool selected_) : base(section_, name_, description_, extensionName_, permissionEnumValue_, permissionEnumType_, moduleName_)
        {
            Selected = selected_;
        }
        
        /// <summary>
        /// Constructor, from a <see cref="PermissionDisplay"/>.
        /// </summary>
        /// <param name="permissionDisplay_">Permission display.</param>
        /// <param name="selected_">Selection indicator.</param>
        public SelectablePermissionDisplay(PermissionDisplay permissionDisplay_, bool selected_) : base(permissionDisplay_.Section, permissionDisplay_.ShortName, permissionDisplay_.Description, permissionDisplay_.ExtensionName, permissionDisplay_.PermissionEnumValue, permissionDisplay_.PermissionEnumType, permissionDisplay_.ModuleName)
        {
            Selected = selected_;
        }
    }
}