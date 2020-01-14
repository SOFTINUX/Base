// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
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
            Assert.Throws<NotSupportedException>(() => typeof(Permissions).VerifyThatTypeContainsEnumValue(Int16.MinValue));
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => typeof(OtherPermissions).VerifyThatTypeContainsEnumValue((short)Permissions.Read));
        }
    }
}
