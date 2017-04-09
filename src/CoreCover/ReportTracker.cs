using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CoreCover
{
    public static partial class ReportTracker
    {
        private static readonly Report _report;

        static ReportTracker()
        {
            _report = new Report();
        }

        public static void ReportLine(string fileName, int lineNumber)
        {
            Debug.WriteLine($"{fileName}: {lineNumber}");
            _report.AddLine(fileName, lineNumber);
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
