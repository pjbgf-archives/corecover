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

            Assert.Equal(1, method.Summary.VisitedMethods);
            Assert.Equal(25, method.Summary.SequenceCoverage);
            Assert.Equal(4, method.Summary.NumSequencePoints);
            Assert.Equal(1, method.Summary.VisitedSequencePoints);
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

            Assert.True(method.Visited);
            Assert.Equal(25, method.SequenceCoverage);
            Assert.Equal(4, method.SequencePoints.Length);
        }

        [Fact]
        public void Consolidate_Method_Summary_For_Not_Visited_Sequence_Points()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(5, 0)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(0, method.Summary.VisitedMethods);
            Assert.Equal(0, method.Summary.SequenceCoverage);
            Assert.Equal(5, method.Summary.NumSequencePoints);
            Assert.Equal(0, method.Summary.VisitedSequencePoints);
        }
    }
}
