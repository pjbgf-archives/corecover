using System;
using CoreCover.Sample.Library;
using Xunit;

namespace CoreCover.Sample.Tests
{
    public class PartiallyCoveredTest
    {
        [Fact]
        public void TestOneMethod()
        {
            var partiallyCovered = new PartiallyCovered();
            Assert.True(partiallyCovered.TestedMethod(true));
        }
    }
}
