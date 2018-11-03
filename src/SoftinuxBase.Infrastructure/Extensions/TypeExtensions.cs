// Copyright © 2017 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using SoftinuxBase.Infrastructure.Interfaces;

namespace SoftinuxBase.Infrastructure.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Return the unique string that qualifies the extension.
        /// </summary>
        /// <param name="extensionMetadata_"></param>
        /// <returns>Fully qualified assembly name, given by Assembly.GetName().Name</returns>
        public static string GetExtensionName(this IExtensionMetadata extensionMetadata_)
        {
            return extensionMetadata_.GetType().Assembly.GetName().Name;
        }
    }

}