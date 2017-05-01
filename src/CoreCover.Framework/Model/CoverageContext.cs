// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.Collections.Generic;
using System.Linq;
using CoreCover.Framework.CodeAnalysis;
using CoreCover.Instrumentation;

namespace CoreCover.Framework.Model
{
    public class CoverageContext
    {
        public List<Module> Modules { get; }

        public CoverageContext()
        {
            Modules = new List<Module>();
            StaticAnalysisSummary = new StaticAnalysisSummary();
            TestExecutionSummary = new TestExecutionSummary();
        }

        public StaticAnalysisSummary StaticAnalysisSummary { get; }
        public TestExecutionSummary TestExecutionSummary { get; }

        public void AddModule(Module module)
        {
            Modules.Add(module);
        }

        public void MarkExecution(ExecutedLine executedLine)
        {
            //TODO: Requires refactoring
            var module = Modules.First(x => x.ModuleHash == executedLine.ModuleHash);
            var @class = module.Types.First(x => x.Methods.Any(y => y.MetadataToken == executedLine.MetadataToken));
            var method = @class.Methods.First(y => y.MetadataToken == executedLine.MetadataToken);

            method.Executed = true;

            var sequencePoints = method.SequencePoints.Where(x => executedLine.StartLineNumber == x.StartLine);

            foreach (var sequencePoint in sequencePoints)
                sequencePoint.ExecutionCount++;
        }
    }
}
