using System.Linq;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Adapters
{
    public class CoverageStats
    {
        public void Consolidate(CoverageSession coverageSession)
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
        }

        private static void ProcessMethod(Method method)
        {
            method.Summary = new Summary
            {
                NumSequencePoints = method.SequencePoints.Length,
                VisitedSequencePoints = method.SequencePoints.Count(x => x.VisitCount > 0)
            };

            ProcessMethodSequenceCoverage(method);
            method.Visited = method.SequenceCoverage > 0;
        }

        private static void ProcessMethodSequenceCoverage(Method method)
        {
            if (method.Summary.NumSequencePoints > 0)
                method.Summary.SequenceCoverage = 100 / method.Summary.NumSequencePoints *
                                                  method.Summary.VisitedSequencePoints;

            method.Summary.VisitedMethods = method.Summary.SequenceCoverage > 0 ? 1 : 0;
            method.SequenceCoverage = method.Summary.SequenceCoverage;
        }
    }
}