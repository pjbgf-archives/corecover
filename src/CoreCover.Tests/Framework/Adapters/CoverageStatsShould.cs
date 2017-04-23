using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCover.Framework.Model;
using Xunit;

namespace CoreCover.Tests.Framework.Adapters
{
    public class CoverageStatsShould
    {
        [Fact]
        public void Consolidate_Method_Summary_Based_On_Sequence_Points()
        {
            var coverageSession = new CoverageSession
            {
                Modules = new[]
                {
                    new Module
                    {
                        Classes = new[]
                        {
                            new Class
                            {
                                Methods = new[]
                                {
                                    new Method
                                    {
                                        SequencePoints = new[]
                                        {
                                            new SequencePoint {VisitCount = 1},
                                            new SequencePoint(),
                                            new SequencePoint(),
                                            new SequencePoint()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var coverageStats = new CoverageStats();

            var consolidatedCoverageSession = coverageStats.Consolidate(coverageSession);
            var method = consolidatedCoverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(25, method.Summary.SequenceCoverage);
        }
        [Fact]
        public void Consolidate_Method_SequenceCoverage_And_Summary_SequenceCoverage()
        {
            var coverageSession = new CoverageSession
            {
                Modules = new[]
                {
                    new Module
                    {
                        Classes = new[]
                        {
                            new Class
                            {
                                Methods = new[]
                                {
                                    new Method
                                    {
                                        SequencePoints = new[]
                                        {
                                            new SequencePoint {VisitCount = 1},
                                            new SequencePoint(),
                                            new SequencePoint(),
                                            new SequencePoint()
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var coverageStats = new CoverageStats();

            var consolidatedCoverageSession = coverageStats.Consolidate(coverageSession);
            var method = consolidatedCoverageSession.Modules.First().Classes.First().Methods.First();

            Assert.Equal(25, method.SequenceCoverage);
        }
    }

    public class CoverageStats
    {
        public CoverageSession Consolidate(CoverageSession coverageSession)
        {
            foreach (var module in coverageSession.Modules)
            {
                foreach (var moduleClass in module.Classes)
                {
                    foreach (var method in moduleClass.Methods)
                    {
                        ProcessMethod(method);
                    }
                }
            }

            return coverageSession;
        }

        private static void ProcessMethod(Method method)
        {
            method.Summary = new Summary
            {
                NumSequencePoints = method.SequencePoints.Length,
                VisitedSequencePoints = method.SequencePoints.Count(x => x.VisitCount > 0),
            };

            if (method.Summary.NumSequencePoints > 0)
                method.Summary.SequenceCoverage = 100 / method.Summary.NumSequencePoints * 
                    method.Summary.VisitedSequencePoints;
            
            method.SequenceCoverage = method.Summary.SequenceCoverage;
        }
    }
}
