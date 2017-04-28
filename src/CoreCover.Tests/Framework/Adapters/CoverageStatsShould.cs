using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreCover.Framework.Adapters;
using CoreCover.Tests.Builders;
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
                .AddMethod(4, 1, 0, 0)
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
                .AddMethod(4, 1, 0, 0)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.True(method.Visited);
            Assert.Equal(25, method.SequenceCoverage);
            Assert.Equal(4, method.SequencePoints.Length);
        }

        [Fact]
        public void Consolidate_Method_Summary_For_Unvisited_Sequence_Points()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(5, 0, 0, 0)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(0, method.Summary.VisitedMethods);
            Assert.Equal(0, method.Summary.SequenceCoverage);
            Assert.Equal(5, method.Summary.NumSequencePoints);
            Assert.Equal(0, method.Summary.VisitedSequencePoints);
        }

        [Fact]
        public void Consolidate_Method_Summary_Based_On_Branch_Points()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(0, 0, 4, 1)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var method = coverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(1, method.Summary.VisitedMethods);
            Assert.Equal(1, method.Summary.VisitedClasses);
            Assert.Equal(25, method.Summary.BranchCoverage);
            Assert.Equal(4, method.Summary.NumBranchPoints);
            Assert.Equal(1, method.Summary.VisitedBranchPoints);
        }

        [Fact]
        public void Consolidate_Class_Summary_Based_On_Methods()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(4, 2, 0, 0)
                .AddMethod(2, 2, 1, 1)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var classType = coverageSession.Modules.First().Classes.First();

            Assert.Equal(2, classType.Summary.VisitedMethods);
            Assert.Equal(75, classType.Summary.SequenceCoverage);
            Assert.Equal(100, classType.Summary.BranchCoverage);
            Assert.Equal(6, classType.Summary.NumSequencePoints);
            Assert.Equal(4, classType.Summary.VisitedSequencePoints);
            Assert.Equal(1, classType.Summary.NumBranchPoints);
            Assert.Equal(1, classType.Summary.VisitedBranchPoints);
        }


        [Fact]
        public void Consolidate_Module_Summary_Based_On_Classes()
        {
            var coverageStats = new CoverageStats();
            var coverageSession = CoverageSessionBuilder.New()
                .AddModule()
                .AddClass()
                .AddMethod(1, 1, 1, 1)
                .AddClass()
                .AddMethod(2, 1, 2, 1)
                .AddClass()
                .AddMethod(4, 2, 4, 2)
                .Build();

            coverageStats.Consolidate(coverageSession);
            var module = coverageSession.Modules.First();

            Assert.Equal(3, module.Summary.VisitedMethods);
            Assert.Equal(66.666666666666666666666666667M, module.Summary.SequenceCoverage);
            Assert.Equal(66.666666666666666666666666667M, module.Summary.BranchCoverage);
            Assert.Equal(7, module.Summary.NumSequencePoints);
            Assert.Equal(4, module.Summary.VisitedSequencePoints);
            Assert.Equal(7, module.Summary.NumBranchPoints);
            Assert.Equal(4, module.Summary.VisitedBranchPoints);
        }
    }
}
