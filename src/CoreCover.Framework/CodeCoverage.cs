using System;
using System.Collections.Generic;
using System.IO;
using Grpc.Core;
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

            //TODO: SRP violation. Whose responsibility is this?
            var server = new Server
            {
                Services = { ExecutionTracker.BindService(new ExecutionTrackerServer(coverageSession)) },
                Ports = { new ServerPort("localhost", 50051, ServerCredentials.Insecure) }
            };
            server.Start();

            //HACK: All paths should come from within the project file.
            var fullPath = testProjectOutputPath;
            if (!Path.IsPathRooted(testProjectOutputPath))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), testProjectOutputPath);

            var testProjectPath = Directory.GetParent(fullPath).Parent.Parent.Parent.FullName;
            _testRunner.Run(testProjectPath);

            server.ShutdownAsync();

            _coverageReport.Export(coverageSession, reportPath);
        }
    }
}