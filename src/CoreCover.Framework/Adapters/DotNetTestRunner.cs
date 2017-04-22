// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using CoreCover.Framework.Abstractions;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Adapters
{
    public class DotNetTestRunner : ITestsRunner
    {
        private readonly IRpcServer _rpcServer;
        private readonly IProcess _process;

        public DotNetTestRunner(IRpcServer rpcServer, IProcess process)
        {
            _rpcServer = rpcServer;
            _process = process;
        }

        public void Run(CoverageSession coverageSession, string testProjectOutputPath)
        {
            var testProjectPath = GetTestProjectPath(testProjectOutputPath);

            _rpcServer.Start(coverageSession);

            /*
             * It is important that the --no-build flag is used, otherwise dotnet-test will run a new build of the project 
             * which will overwrite the assembly files that were instrumented.
             */
            _process.Execute("dotnet", "test --no-build", testProjectPath);

            _rpcServer.Stop();
        }

        private static string GetTestProjectPath(string testProjectOutputPath)
        {
            var fullPath = testProjectOutputPath;
            if (!Path.IsPathRooted(testProjectOutputPath))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), testProjectOutputPath);

            var testProjectPath = Directory.GetParent(fullPath).Parent.Parent.Parent.FullName;
            return testProjectPath;
        }
    }
}