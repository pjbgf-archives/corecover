// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework;
using CoreCover.Framework.Abstractions;
using NSubstitute;
using OpenCover.Framework.Model;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class CodeCoverageShould
    {
        private readonly ITestsRunner _testRunnerMock;
        private readonly ICoverageReport _coverageReportMock;
        private readonly IInstrumentator _instrumentatorMock;
        private readonly IRpcServer _rpcServerMock;

        public CodeCoverageShould()
        {
            _testRunnerMock = Substitute.For<ITestsRunner>();
            _coverageReportMock = Substitute.For<ICoverageReport>();
            _instrumentatorMock = Substitute.For<IInstrumentator>();
            _rpcServerMock = Substitute.For<IRpcServer>();
        }

        [Fact]
        public void Instrument_Code_Before_Starting_Rpc()
        {
            var codeCoverage = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock, _rpcServerMock);

            codeCoverage.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _instrumentatorMock.Process(Arg.Any<CoverageSession>(), Arg.Any<string>());
                _rpcServerMock.Start(Arg.Any<CoverageSession>());
            });
        }

        [Fact]
        public void Start_Rpc_Server_Before_Running_Tests()
        {
            var codeCoverage = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock, _rpcServerMock);

            codeCoverage.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _rpcServerMock.Start(Arg.Any<CoverageSession>());
                _testRunnerMock.Run(Arg.Any<string>());
            });
        }

        [Fact]
        public void Stop_Rpc_Server_After_Running_Tests()
        {
            var codeCoverage = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock, _rpcServerMock);

            codeCoverage.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _testRunnerMock.Run(Arg.Any<string>());
                _rpcServerMock.Stop();
            });
        }

        [Fact]
        public void Generate_Report_Once_Tests_Were_Executed()
        {
            var codeCoverage = new CoverageRunner(_instrumentatorMock, _testRunnerMock, _coverageReportMock, _rpcServerMock);

            codeCoverage.Run("testProjectOutputPath", "report.xml");

            Received.InOrder(() =>
            {
                _testRunnerMock.Run(Arg.Any<string>());
                _coverageReportMock.Export(Arg.Any<CoverageSession>(), Arg.Any<string>());
            });
        }
    }
}