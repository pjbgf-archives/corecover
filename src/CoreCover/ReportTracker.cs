using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoreCover
{
    public static class ReportTracker
    {
        public static void ReportLine(string fileName, int lineNumber)
        {
            Debug.WriteLine($"{fileName}: {lineNumber}");
            File.AppendAllLines("C:\\git\\corecover\\src\\CoreCover.Sample.Tests\\bin\\Debug\\netcoreapp1.1\\report.xml", new string[] { $"{fileName}: {lineNumber}" });
        }
    }
}
