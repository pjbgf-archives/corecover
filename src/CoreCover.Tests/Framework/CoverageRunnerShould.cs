// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework;
using CoreCover.Framework.Abstractions;
using NSubstitute;
using OpenCover.Framework.Model;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class CoverageRunnerShould
    {
        private readonly ITestsRunner _testRunnerMock;
        private readonly ICoverageReport _coverageReportMock;
        private readonly IInstrumentator _instrumentatorMock;

        public CoverageRunnerShould()
        {
            _testRunnerMock = Substitute.For<ITestsRunner>();
            _coverageReportMock = Substitute.For<ICoverageReport>();
            _instrumentatorMock = Substitute.For<IInstrumentator>();
        }

        [Fact]
        public void Instrument_Code_Before_Executing_Tests()
        {
            var coverageRunner = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock);

            coverageRunner.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _instrumentatorMock.Process(Arg.Any<CoverageSession>(), Arg.Any<string>());
                _testRunnerMock.Run(Arg.Any<CoverageSession>(), Arg.Any<string>());
            });
        }

        [Fact]
        public void Generate_Report_Once_Tests_Were_Executed()
        {
            var coverageRunner = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock);

            coverageRunner.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _testRunnerMock.Run(Arg.Any<CoverageSession>(), Arg.Any<string>());
                _coverageReportMock.Export(Arg.Any<CoverageSession>(), Arg.Any<string>());
            });
        }

        [Fact]
        public void Use_Default_Report_File_Name_If_None_Is_Provided()
        {
            var coverageRunner = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock);

            coverageRunner.Run("testProjectOutputPath", string.Empty);

            _coverageReportMock.Received(1).Export(Arg.Any<CoverageSession>(), 
                Arg.Is<string>(s => s.EndsWith("coverage.xml"))
            );
        }
    }
}