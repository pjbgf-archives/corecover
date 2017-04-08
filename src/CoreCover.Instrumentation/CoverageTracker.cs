using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace CoreCover.Instrumentation
{
    public static class CoverageTracker
    {
        public static void MarkExecution(string fileName, int lineNumber)
        {
            Debug.WriteLine($"{fileName}: {lineNumber}");
            File.AppendAllLines("C:\\git\\corecover\\src\\CoreCover.Sample.Tests\\bin\\Debug\\netcoreapp1.1\\coverage.xml", new string[] { $"{fileName}: {lineNumber}" });
        }
    }
}
