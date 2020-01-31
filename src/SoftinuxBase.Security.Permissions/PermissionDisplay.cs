// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/ and 2017-2019 SOFTINUX.
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using SoftinuxBase.Security.Common;

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
        public string ExtensionName { get; private set; }
        /// <summary>
        /// GroupName, which groups permissions working in the same area
        /// </summary>
        public string GroupName { get; private set; }
        /// <summary>
        /// ShortName of the permission - often says what it does, e.g. Read
        /// </summary>
        public string ShortName { get; private set; }
        /// <summary>
        /// Long description of what action this permission allows 
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Gives the actual permission
        /// </summary>
        public short Permission { get; private set; }
        /// <summary>
        /// Contains an optional paidForModule that this feature is linked to
        /// </summary>
        public string ModuleName { get; private set; }


        /// <summary>
        /// This returns 
        /// </summary>
        /// <returns></returns>
        public static List<PermissionDisplay> GetPermissionsToDisplay(Type enumType_) 
        {
            var result = new List<PermissionDisplay>();
            foreach (var permissionName in Enum.GetNames(enumType_))
            {
                var member = enumType_.GetMember(permissionName);
                //This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;
                //If there is no DisplayAttribute then the Enum is not used
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;

                //Gets the optional PaidForModule that a permission can be linked to
                var moduleAttribute = member[0].GetCustomAttribute<LinkedToModuleAttribute>();

                var permission = (short)Enum.Parse(enumType_, permissionName, false);

                result.Add(new PermissionDisplay(displayAttribute.GroupName, displayAttribute.Name, 
                        displayAttribute.Description, enumType_.FullName, permission, moduleAttribute?.PaidForModule.ToString()));
            }

            return result;
        }
    }
}