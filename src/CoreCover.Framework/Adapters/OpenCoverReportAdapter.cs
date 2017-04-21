// MIT License
// Copyright (c) 2017 Paulo Gomes

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
            GenerateReport(coverageSession, reportPath);
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