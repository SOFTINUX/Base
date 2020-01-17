// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;

namespace SoftinuxBase.Security.PermissionParts
{
    public static class StringExtensions
    {

        /// <summary>
        /// Gets the elements of the policy name.
        /// </summary>
        /// <param name="policyName_">Policy name.</param>
        /// <returns>The type fullname and permission short value.</returns>
        public static (string, short) ParsePolicyName(this string policyName_)
        {
            var splitted = policyName_.Split('|');
            return (splitted[0], short.Parse(splitted[1]));
        }

        /// <summary>
        /// Gets The name of the policy associated to this permission: an unique identifier generated from the permission assembly and value.
        /// </summary>
        /// <param name="permissionEnumType_"></param>
        /// <param name="permission_"></param>
        /// <returns>Policy name.</returns>
        public static string ToPolicyName(Type permissionEnumType_, short permission_)
        {
            return $"{permissionEnumType_.GetAssemblyShortName()}|{permission_.ToString()}";
        }
    }
}
