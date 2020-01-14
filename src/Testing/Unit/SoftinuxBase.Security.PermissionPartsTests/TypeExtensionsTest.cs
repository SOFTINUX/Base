// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using SoftinuxBase.Security.PermissionParts;
using Xunit;

namespace SoftinuxBase.Security.PermissionPartsTests
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void VerifyThatTypeContainsEnumValue_Ok()
        {
            typeof(Permissions).VerifyThatTypeContainsEnumValue((short)Permissions.AccessAll);
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_NotSupportedException()
        {
            // TODO
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_ArgumentNullException()
        {
            // TODO
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_ArgumentException()
        {
            // TODO
        }
    }
}
