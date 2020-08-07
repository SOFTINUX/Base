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
        public PermissionDisplay(string section_, string name_, string description_, string extensionName_, short permissionEnumValue_,
            Type permissionEnumType_, string moduleName_)
        {
            ExtensionName = extensionName_;
            PermissionEnumValue = permissionEnumValue_;
            PermissionEnumType = permissionEnumType_;
            Section = section_;
            ShortName = name_ ?? throw new ArgumentNullException(nameof(name_));
            Description = description_ ?? throw new ArgumentNullException(nameof(description_));
            ModuleName = moduleName_;
        }

        /// <summary>
        /// Extension name.
        /// </summary>
        public string ExtensionName { get; }

        /// <summary>
        /// Label of section, which groups permissions in a functionality area.
        /// </summary>
        public string Section { get; }

        /// <summary>
        /// ShortName of the permission - often says what it does, e.g. Read.
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Long description of what action this permission allows.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gives the actual permission (enum value).
        /// </summary>
        public short PermissionEnumValue { get; }

        /// <summary>
        /// The C# Type that defines the actual permission (enum assembly-qualified type).
        /// </summary>
        public Type PermissionEnumType { get; }

        /// <summary>
        /// Contains an optional paidForModule that this feature is linked to.
        /// </summary>
        public string ModuleName { get; }


        /// <summary>
        /// This returns the non-obsolete permissions from an enum Type.
        /// </summary>
        /// <param name="extensionName_">The name of the extension, since permissions can be defined anywhere in assemblies.</param>
        /// <param name="enumType_">Type that should be an enum with Display attribute.</param>
        /// <param name="enumValues_">Optional values to build PermissionDisplay from these values only.</param>
        /// <returns></returns>
        public static List<PermissionDisplay> GetPermissionsToDisplay(string extensionName_, Type enumType_, HashSet<short> enumValues_ = null)
        {
            var result = new List<PermissionDisplay>();
            foreach (var permissionName in Enum.GetNames(enumType_))
            {
                var member = enumType_.GetMember(permissionName);
                // This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;
                // If there is no DisplayAttribute then the value is not managed in administration interface
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;

                // Gets the optional PaidForModule that a permission can be linked to
                var moduleAttribute = member[0].GetCustomAttribute<LinkedToModuleAttribute>();

                var permission = Convert.ToInt16(Enum.Parse(enumType_, permissionName, false));

                if (enumValues_ == null || enumValues_.Contains(permission))
                {
                    result.Add(new PermissionDisplay(displayAttribute.GroupName, displayAttribute.Name,
                        displayAttribute.Description, extensionName_, permission, enumType_,
                        moduleAttribute?.PaidForModule.ToString()));
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PermissionDisplay;
            if (other == null)
            {
                return false;
            }

            return this.ExtensionName == other.ExtensionName && this.Section == other.Section && this.ShortName == other.ShortName && this.PermissionEnumValue == other.PermissionEnumValue;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.ExtensionName, this.Section, this.ShortName, this.PermissionEnumValue);
        }

        public override string ToString()
        {
            return $"[{ExtensionName}][{Section}][{ShortName}][{PermissionEnumValue.ToString()}]";
        }
    }
}