using System;
using NetCover.Sample.Library;
using Xunit;

namespace NetCover.Sample.Tests
{
    public class PartiallyCoveredTest
    {
        [Fact]
        public void TestOneMethod()
        {
            var partiallyCovered = new PartiallyCovered();
            Assert.True(partiallyCovered.TestedMethod());
        }
    }
}
