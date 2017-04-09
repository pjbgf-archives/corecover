using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Loader;

namespace CoreCover.Instrumentation
{
    public static class CoverageTracker
    {
        private static bool _unloadMethodSetFlag = false;
        
        private static void SetUnloadingDelegate()
        {
            if (!_unloadMethodSetFlag)
            {
                _unloadMethodSetFlag = true;
                //HACK: Need to find a better way to loading/keeping report state
                ReportTracker.LoadReport();
                AssemblyLoadContext.Default.Unloading += Default_Unloading;
            }
        }

        private static void Default_Unloading(AssemblyLoadContext obj)
        {
            ReportTracker.WriteReport();
        }

        public static void MarkExecution(string fileName, int lineNumber)
        {
            if (!_unloadMethodSetFlag)
                SetUnloadingDelegate();

            Debug.WriteLine($"{fileName}: {lineNumber}");
            ReportTracker.MarkLineAsCovered(fileName, lineNumber);
        }
    }
}
