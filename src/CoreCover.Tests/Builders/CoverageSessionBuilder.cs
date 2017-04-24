using System.Linq;
using OpenCover.Framework.Model;

namespace CoreCover.Tests.Builders
{
    internal class CoverageSessionBuilder
    {
        private CoverageSession _coverageSession;

        public static CoverageSessionBuilder New()
        {
            return new CoverageSessionBuilder { _coverageSession = new CoverageSession() };
        }

        public CoverageSessionBuilder AddModule()
        {
            _coverageSession.Modules = new[]
            {
                new Module
                {
                }
            };

            return this;
        }

        public CoverageSessionBuilder AddClass()
        {
            _coverageSession.Modules.First<Module>().Classes = new[]
            {
                new Class
                {

                }
            };

            return this;
        }

        public CoverageSessionBuilder AddMethod(int numSequencePoints, int visitedSequencePoints, int numBranchPoints, int visitedBranchPoints)
        {
            var sequencePoints = Enumerable.Range(0, numSequencePoints).Select(x => new SequencePoint()).ToArray();
            for (var i = 0; i < visitedSequencePoints; i++)
            {
                sequencePoints[i].VisitCount = 1;
            }

            var branchPoints = Enumerable.Range(0, numBranchPoints).Select(x => new BranchPoint()).ToArray();
            for (var i = 0; i < visitedBranchPoints; i++)
            {
                branchPoints[i].VisitCount = 1;
            }

            var method = new Method { SequencePoints = sequencePoints, BranchPoints = branchPoints };
            _coverageSession.Modules.First().Classes.First().Methods = new[] { method };

            return this;
        }

        public CoverageSession Build()
        {
            return _coverageSession;
        }
    }
}