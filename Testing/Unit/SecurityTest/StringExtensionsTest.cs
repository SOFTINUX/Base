using Security.Extensions;
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
        public void Test(string value)
        {
            Assert.Equal("Thisisatest", value.UppercaseFirst());
        }
    }
}