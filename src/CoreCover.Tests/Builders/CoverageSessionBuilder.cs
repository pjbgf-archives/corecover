using System.Collections.Generic;
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
            var classes = new List<Class>();
            var module = _coverageSession.Modules.Last();

            if (module.Classes != null)
                classes.AddRange(module.Classes);

            classes.Add(new Class());

            module.Classes = classes.ToArray();

            return this;
        }

        public CoverageSessionBuilder AddMethod(int numSequencePoints, int visitedSequencePoints, int numBranchPoints, int visitedBranchPoints)
        {
            var methods = new List<Method>();
            var classtype = _coverageSession.Modules.Last().Classes.Last();

            if (classtype.Methods != null)
                methods.AddRange(classtype.Methods);

            var method = GetMethod(numSequencePoints, visitedSequencePoints, numBranchPoints, visitedBranchPoints);
            methods.Add(method);

            classtype.Methods = methods.ToArray();

            return this;
        }

        private static Method GetMethod(int numSequencePoints, int visitedSequencePoints, int numBranchPoints,
            int visitedBranchPoints)
        {
            var sequencePoints = GetSequencePoints(numSequencePoints, visitedSequencePoints);
            var branchPoints = GetBranchPoints(numBranchPoints, visitedBranchPoints);
            var method = new Method { SequencePoints = sequencePoints, BranchPoints = branchPoints };

            return method;
        }

        private static BranchPoint[] GetBranchPoints(int numBranchPoints, int visitedBranchPoints)
        {
            var branchPoints = Enumerable.Range(0, numBranchPoints).Select(x => new BranchPoint()).ToArray();
            for (var i = 0; i < visitedBranchPoints; i++)
                branchPoints[i].VisitCount = 1;

            return branchPoints;
        }

        private static SequencePoint[] GetSequencePoints(int numSequencePoints, int visitedSequencePoints)
        {
            var sequencePoints = Enumerable.Range(0, numSequencePoints).Select(x => new SequencePoint()).ToArray();
            for (var i = 0; i < visitedSequencePoints; i++)
                sequencePoints[i].VisitCount = 1;

            return sequencePoints;
        }

        public CoverageSession Build()
        {
            return _coverageSession;
        }
    }
}