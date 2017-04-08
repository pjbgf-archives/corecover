using System;
using System.Collections.Generic;
using System.Text;
using CoreCover.Instrumentation;

namespace CoreCover.Sample.Library
{
    public class InstrumentedSample
    {
            public bool TestedMethod()
            {
                CoverageTracker.MarkExecution("C:\\git\\corecover\\src\\CoreCover.Sample.Library\\InstrumentedSample.cs", 13);
                return true;
            }
        }
}
