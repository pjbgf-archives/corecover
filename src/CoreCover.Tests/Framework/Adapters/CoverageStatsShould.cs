using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreCover.Framework.Adapters;
using CoreCover.Tests.Data;
using NSubstitute.Routing.Handlers;
using Xunit;

namespace CoreCover.Tests.Framework.Adapters
{
    public class CoverageStatsShould
    {
        [Fact]
        public void Consolidate_Method_Summary_Based_On_Sequence_Points()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(4, 1)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(25, method.Summary.SequenceCoverage);
        }

        [Fact]
        public void Consolidate_Method_SequenceCoverage_And_Summary_SequenceCoverage()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(4, 1)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(25, method.SequenceCoverage);
        }
    }
}
