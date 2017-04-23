// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using System.Text;
using System.Xml.Serialization;
using CoreCover.Framework.Abstractions;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Adapters
{
    public class OpenCoverReportAdapter : ICoverageReport
    {
        public void Export(CoverageSession coverageSession, string reportPath)
        {
            ConsolidateSummaries(coverageSession);
            GenerateReport(coverageSession, reportPath);
        }

        private void ConsolidateSummaries(CoverageSession coverageSession)
        {
            foreach (var module in coverageSession.Modules)
            {
                foreach (var moduleClass in module.Classes)
                {
                    foreach (var method in moduleClass.Methods)
                    {
                        foreach (var sequencePoint in method.SequencePoints)
                        {
                            method.Summary.VisitedSequencePoints += sequencePoint.VisitCount > 0 ? 1 : 0;
                        }

                        method.Summary.VisitedMethods = method.Summary.VisitedSequencePoints > 0 ? 1 : 0;
                        if (method.Summary.NumSequencePoints > 0)
                            method.Summary.BranchCoverage = 100 / method.Summary.NumSequencePoints * method.Summary.VisitedMethods;

                        method.BranchCoverage = method.Summary.BranchCoverage;

                        moduleClass.Summary.NumSequencePoints += method.SequencePoints.Length;
                        moduleClass.Summary.VisitedMethods += method.Summary.VisitedMethods;
                        if (moduleClass.Summary.NumSequencePoints > 0)
                            moduleClass.Summary.BranchCoverage = 100 / moduleClass.Summary.NumSequencePoints * moduleClass.Summary.VisitedMethods;

                        module.Summary.NumSequencePoints += method.SequencePoints.Length;
                        module.Summary.VisitedMethods += method.Summary.VisitedMethods;
                        if (module.Summary.NumSequencePoints > 0)
                            module.Summary.BranchCoverage = 100 / module.Summary.NumSequencePoints * module.Summary.VisitedMethods;
                    }

                    moduleClass.Summary.VisitedClasses = moduleClass.Summary.VisitedMethods > 0 ? 1 : 0;
                    module.Summary.VisitedClasses += moduleClass.Summary.VisitedClasses;
                }
            }
        }

        private void GenerateReport(CoverageSession coverageSession, string reportPath)
        {
            var serializer = new XmlSerializer(typeof(CoverageSession),
                new[] { typeof(OpenCover.Framework.Model.Module), typeof(OpenCover.Framework.Model.File), typeof(Class) });
            ExportReport(reportPath, serializer, coverageSession);
        }

        private void ExportReport(string reportPath, XmlSerializer serializer, CoverageSession openCoverReport)
        {
            using (var stream = new FileStream(reportPath, FileMode.Create))
            using (var writer = new StreamWriter(stream, new UTF8Encoding()))
            {
                serializer.Serialize(writer, openCoverReport);
            }
        }
    }
}