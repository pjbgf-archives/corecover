// MIT License
// Copyright (c) 2017 Paulo Gomes

using System;
using System.Collections.Generic;
using System.IO;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public partial class CodeCoverage : ICodeCoverage
    {
        private readonly ITestsRunner _testRunner;
        private readonly IInstrumentator _instrumentator;
        private readonly ICoverageReport _coverageReport;
        private readonly IRpcServer _rpcServer;

        public CodeCoverage(IInstrumentator instrumentator, ITestsRunner testRunner, ICoverageReport coverageReport, IRpcServer rpcServer)
        {
            _instrumentator = instrumentator;
            _coverageReport = coverageReport;
            _rpcServer = rpcServer;
            _testRunner = testRunner;
        }

        public void Run(string testProjectOutputPath, string reportPath)
        {
            var coverageSession = new CoverageSession();

            _instrumentator.Process(coverageSession, testProjectOutputPath);

            _rpcServer.Start(coverageSession);

            _testRunner.Run(testProjectOutputPath);

            _rpcServer.Stop();

            _coverageReport.Export(coverageSession, reportPath);
        }
    }
}