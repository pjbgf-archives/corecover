using System;
using System.Collections.Generic;
using System.Text;

namespace CoreCover.Framework.Model
{
    public class TestExecutionSummary
    {
        public int NumberOfAssembliesVisited { get; set; }
        public int NumberOfTypesVisited { get; set; }
        public int NumberOfMethodsVisited { get; set; }
        public decimal CodeCoverageRatio { get; set; }
    }
}
