// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

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
