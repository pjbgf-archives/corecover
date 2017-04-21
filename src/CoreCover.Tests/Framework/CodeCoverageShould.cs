// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework;
using NSubstitute;
using OpenCover.Framework.Model;
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

            testRunnerMock.Received(1).Run(Arg.Any<string>());
            instrumentatorMock.Received(1).Process(Arg.Any<CoverageSession>(), Arg.Is("testProjectOutputPath"));
            coverageReportMock.Received(1).Export(Arg.Any<CoverageSession>(), Arg.Is("report.xml"));
        }
    }
}