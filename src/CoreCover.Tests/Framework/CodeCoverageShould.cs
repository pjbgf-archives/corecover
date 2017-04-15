using CoreCover.Framework;
using NSubstitute;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class CodeCoverageShould
    {
        [Fact]
        public void Generate_Report_After_Successful_Execution()
        {
            var testRunnerMock = Substitute.For<ITestsRunner>();
            var coverageReportMock = Substitute.For<ICoverageReport>();
            var instrumentatorMock = Substitute.For<IInstrumentator>();
            var codeCoverage = new CodeCoverage(instrumentatorMock, testRunnerMock, coverageReportMock);

            codeCoverage.Run("testProjectOutputPath", "report.xml");

            testRunnerMock.Received(1).Run(Arg.Is("testProjectOutputPath"));
            coverageReportMock.Received(1).Export(Arg.Is("report.xml"));
            instrumentatorMock.Received(1).Process(Arg.Is("testProjectOutputPath"));
        }
    }
}