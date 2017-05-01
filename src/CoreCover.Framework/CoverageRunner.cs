// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Model;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class CoverageRunner : ICoverageRunner
    {
        private readonly ITestsRunner _testRunner;
        private readonly IInstrumentator _instrumentator;
        private readonly ICoverageReport _coverageReport;

        public CoverageRunner(IInstrumentator instrumentator, ITestsRunner testRunner, ICoverageReport coverageReport)
        {
            _instrumentator = instrumentator;
            _testRunner = testRunner;
            _coverageReport = coverageReport;
        }

        public void Run(string testProjectOutputPath, string reportPath)
        {
            if (string.IsNullOrEmpty(reportPath))
                reportPath = "coverage.xml";

            var coverageContext = new CoverageContext();
            _instrumentator.Process(coverageContext, testProjectOutputPath);
            _testRunner.Run(coverageContext, testProjectOutputPath);
            _coverageReport.Export(coverageContext, reportPath);
        }
    }
}