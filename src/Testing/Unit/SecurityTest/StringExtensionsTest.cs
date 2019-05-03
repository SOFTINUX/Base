// Copyright © 2017-2019 SOFTINUX. All rights reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE file in the project root for license information.

using SoftinuxBase.Infrastructure.Tools;
using Xunit;

namespace SecurityTest
{
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("thisisatest")]
        [InlineData("ThisIsaTest")]
        [InlineData("Thisisatest")]
        [InlineData("THISISATEST")]
        public void UppercaseFirst(string value_)
        {
            Assert.Equal("Thisisatest", value_.UppercaseFirst());
        }
    }
}