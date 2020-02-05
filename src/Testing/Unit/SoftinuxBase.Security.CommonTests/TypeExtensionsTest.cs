// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using System;
using SoftinuxBase.Security.Common;
using SoftinuxBase.Tests.Common;
using Xunit;

namespace SoftinuxBase.Security.CommonTests
{
    public class TypeExtensionsTest
    {
        [Fact]
        public void VerifyThatTypeContainsEnumValue_Ok()
        {
            typeof(Common.Enums.Permissions).VerifyThatTypeContainsEnumValue((short)Common.Enums.Permissions.AccessAll);
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_NotSupportedException()
        {
            Assert.Throws<NotSupportedException>(() => typeof(Common.Enums.Permissions).VerifyThatTypeContainsEnumValue(Int16.MinValue));
        }

        [Fact]
        public void VerifyThatTypeContainsEnumValue_ArgumentException()
        {
            Assert.Throws<ArgumentException>(() => typeof(OtherPermissions).VerifyThatTypeContainsEnumValue((short)Common.Enums.Permissions.Read));
        }
    }
}
