using CoreCover.Sample.Library;
using Xunit;

namespace CoreCover.Sample.Tests
{
    public class FullyCoveredTest
    {
        [Fact]
        public void TestOneMethod()
        {
            var fullyCovered = new FullyCovered();
            Assert.Equal(4, fullyCovered.TestedMethod(1));
        }
    }
}
