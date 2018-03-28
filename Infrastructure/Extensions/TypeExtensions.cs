// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for license information.

using Infrastructure.Interfaces;

namespace Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Return the string used for the "scope".
        /// </summary>
        /// <param name="extensionMetadata_"></param>
        /// <returns>The scope, equal to assembly name, given by Assembly.GetName().Name</returns>
        public static string GetScope(this IExtensionMetadata extensionMetadata_)
        {
            return extensionMetadata_.GetType().Assembly.GetName().Name;
        }
    }

}