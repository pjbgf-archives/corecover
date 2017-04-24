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
                VisitedSequencePoints = method.SequencePoints.Count(x => x.VisitCount > 0),
                NumBranchPoints = method.BranchPoints.Length,
                VisitedBranchPoints = method.BranchPoints.Count(x => x.VisitCount > 0)
            };

            ProcessMethodSequenceCoverage(method);
            ProcessMethodBranchCoverage(method);

            method.Visited = method.SequenceCoverage > 0 || method.BranchCoverage > 0;
            method.Summary.VisitedMethods = method.Visited ? 1 : 0;
            method.Summary.VisitedClasses = method.Summary.VisitedMethods;
        }

        private static void ProcessMethodBranchCoverage(Method method)
        {
            if (method.Summary.NumBranchPoints > 0)
                method.Summary.BranchCoverage = 100 / method.Summary.NumBranchPoints *
                                                  method.Summary.VisitedBranchPoints;
            
            method.BranchCoverage = method.Summary.BranchCoverage;
        }

        private static void ProcessMethodSequenceCoverage(Method method)
        {
            if (method.Summary.NumSequencePoints > 0)
                method.Summary.SequenceCoverage = 100 / method.Summary.NumSequencePoints *
                                                  method.Summary.VisitedSequencePoints;

            method.SequenceCoverage = method.Summary.SequenceCoverage;
        }
    }
}