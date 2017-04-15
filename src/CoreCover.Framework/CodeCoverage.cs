namespace CoreCover.Framework
{
    public class CodeCoverage : ICodeCoverage
    {
        private readonly ITestsRunner _testRunner;
        private readonly IInstrumentator _instrumentator;
        private readonly ICoverageReport _coverageReport;
        
        public CodeCoverage(IInstrumentator instrumentator, ITestsRunner testRunner, ICoverageReport coverageReport)
        {
            _instrumentator = instrumentator;
            _coverageReport = coverageReport;
            _testRunner = testRunner;
        }

        public void Run(string testProjectOutputPath, string reportPath)
        {
            _instrumentator.Process(testProjectOutputPath);
            _testRunner.Run(testProjectOutputPath);
            _coverageReport.Export(reportPath);
        }
    }
}