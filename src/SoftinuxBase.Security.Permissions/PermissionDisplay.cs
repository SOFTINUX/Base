// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SoftinuxBase.Security.Permissions
{
    public class PermissionDisplay
    {
        public PermissionDisplay(string groupName_, string name_, string description_, string extensionName_, short permission_,
            string moduleName_)
        {
            ExtensionName = extensionName_;
            Permission = permission_;
            GroupName = groupName_;
            ShortName = name_ ?? throw new ArgumentNullException(nameof(name_));
            Description = description_ ?? throw new ArgumentNullException(nameof(description_));
            ModuleName = moduleName_;
        }

        /// <summary>
        /// Extension name.
        /// </summary>
        public string ExtensionName { get; }

        /// <summary>
        /// GroupName, which groups permissions working in the same area.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// ShortName of the permission - often says what it does, e.g. Read.
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Long description of what action this permission allows.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gives the actual permission.
        /// </summary>
        public short Permission { get; }

        /// <summary>
        /// Contains an optional paidForModule that this feature is linked to.
        /// </summary>
        public string ModuleName { get; }


        /// <summary>
        /// This returns the non-obsolete permissions from an enum Type.
        /// </summary>
        /// <param name="enumType_">Type that should be an enum with Display attribute.</param>
        /// <param name="enumValues_">Optional values to build PermissionDisplay from these values only.</param>
        /// <returns></returns>
        public static List<PermissionDisplay> GetPermissionsToDisplay(Type enumType_, HashSet<short> enumValues_ = null)
        {
            var result = new List<PermissionDisplay>();
            foreach (var permissionName in Enum.GetNames(enumType_))
            {
                var member = enumType_.GetMember(permissionName);
                // This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;
                // If there is no DisplayAttribute then the Enum is not used
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;

                // Gets the optional PaidForModule that a permission can be linked to
                var moduleAttribute = member[0].GetCustomAttribute<LinkedToModuleAttribute>();

                var permission = Convert.ToInt16(Enum.Parse(enumType_, permissionName, false));

                if (enumValues_ == null || enumValues_.Contains(permission))
                {
                    result.Add(new PermissionDisplay(displayAttribute.GroupName, displayAttribute.Name,
                        displayAttribute.Description, enumType_.FullName, permission, moduleAttribute?.PaidForModule.ToString()));
                }
            }

            return result;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as PermissionDisplay;
            if (other == null)
            {
                return false;
            }

            return this.ExtensionName == other.ExtensionName && this.GroupName == other.GroupName && this.ShortName == other.ShortName && this.Permission == other.Permission;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ExtensionName, this.GroupName, this.ShortName, this.Permission);
        }

        public override String ToString()
        {
            return $"[{ExtensionName}][{GroupName}][{ShortName}][{Permission.ToString()}]";
        }
    }
}