using System.IO;
using System.Text;
using System.Xml.Serialization;
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
            using (var fs = new FileStream(reportPath, FileMode.Create))
            using (var writer = new StreamWriter(fs, new UTF8Encoding()))
            {
                serializer.Serialize(writer, openCoverReport);
            }
        }
    }
}