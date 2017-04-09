using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CoreCover.Instrumentation
{
    public static class ReportTracker
    {
        private static Report _report;

        static ReportTracker()
        {
            _report = new Report();
        }

        public static void ReportLine(string fileName, int lineNumber)
        {
            Debug.WriteLine($"{fileName}: {lineNumber}");
            _report.AddLine(fileName, lineNumber);
        }

        public static void MarkLineAsCovered(string fileName, int lineNumber)
        {
            _report.MarkLineAsCovered(fileName, lineNumber);
        }

        public static void LoadReport()
        {
            var sw = new StringReader(File.ReadAllText("C:\\git\\corecover\\src\\CoreCover.Sample.Tests\\bin\\Debug\\netcoreapp1.1\\report.xml"));
            var serializer = JsonSerializer.Create(new JsonSerializerSettings { });

            using (var jsonTextWriter = new JsonTextReader(sw))
                _report = serializer.Deserialize<Report>(jsonTextWriter);
        }

        public static void WriteReport()
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var serializer = JsonSerializer.Create(new JsonSerializerSettings { });

            using (var jsonTextWriter = new JsonTextWriter(sw))
                serializer.Serialize(jsonTextWriter, _report);

            File.WriteAllText("C:\\git\\corecover\\src\\CoreCover.Sample.Tests\\bin\\Debug\\netcoreapp1.1\\report.xml", sb.ToString());
        }
    }
}
