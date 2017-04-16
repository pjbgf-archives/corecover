using System.IO;
using OpenCover.Framework.Model;

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
            var coverageSession = new CoverageSession();

            _instrumentator.Process(coverageSession, testProjectOutputPath);

            //HACK: All paths should come from within the project file.
            var fullPath = testProjectOutputPath;
            if (!Path.IsPathRooted(testProjectOutputPath))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), testProjectOutputPath);

            var testProjectPath = Directory.GetParent(fullPath).Parent.Parent.Parent.FullName;
            _testRunner.Run(testProjectPath);

            _coverageReport.Export(coverageSession, reportPath);
        }
    }
}