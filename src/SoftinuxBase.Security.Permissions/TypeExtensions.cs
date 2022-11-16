// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;

namespace SoftinuxBase.Security.Permissions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Verifies that <paramref name="enumValue_"/> matches <paramref name="type_"/>.
        /// </summary>
        /// <param name="type_">Enum type.</param>
        /// <param name="enumValue_">Value to check.</param>
        /// <exception cref="ArgumentNullException"><see cref="Enum.IsDefined"/>.</exception>
        /// <exception cref="InvalidOperationException"><see cref="Enum.IsDefined"/>.</exception>
        /// <exception cref="NotSupportedException"><paramref name="enumValue_"/> is not contained in enumeration of type <paramref name="type_"/></exception>
        public static void VerifyThatTypeContainsEnumValue(this Type type_, short enumValue_)
        {
            if (!Enum.IsDefined(type_, enumValue_))
            {
                throw new NotSupportedException($"Value {enumValue_} is not defined in enum {type_.FullName}");
            }
        }
    }
}
