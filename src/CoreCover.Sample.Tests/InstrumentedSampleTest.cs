using System;
using System.Collections.Generic;
using System.Text;
using CoreCover.Sample.Library;
using Xunit;

namespace CoreCover.Sample.Tests
{
    public class InstrumentedSampleTest
    {
        [Fact]
        public void TestedMethodTest()
        {
            var sample = new InstrumentedSample();
            Assert.True(sample.TestedMethod());
        }
    }
}
