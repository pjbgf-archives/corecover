using System.Linq;
using OpenCover.Framework.Model;

namespace CoreCover.Tests.Data
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
            Enumerable.First<Module>(_coverageSession.Modules).Classes = new[]
            {
                new Class
                {

                }
            };

            return this;
        }

        public CoverageSessionBuilder AddMethod(int numSequencePoints, int visitedSequencePoints)
        {
            var sequencePoints = Enumerable.Range(0, numSequencePoints).Select(x => new SequencePoint()).ToArray();
            for (var i = 0; i < visitedSequencePoints; i++)
            {
                sequencePoints[i].VisitCount = 1;
            }

            var method = new Method { SequencePoints = sequencePoints };
            _coverageSession.Modules.First().Classes.First().Methods = new[] { method };

            return this;
        }

        public CoverageSession Build()
        {
            return _coverageSession;
        }
    }
}