// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Adapters;
using NSubstitute;
using OpenCover.Framework.Model;
using Xunit;

namespace CoreCover.Tests.Framework.Adapters
{
    public class DotNetTestRunnerShould
    {
        private readonly IProcess _process;
        private readonly IRpcServer _rpcServerMock;

        public DotNetTestRunnerShould()
        {
            _process = Substitute.For<IProcess>();
            _rpcServerMock = Substitute.For<IRpcServer>();
        }

        [Fact]
        public void Start_Rpc_Server_Before_Running_Tests()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var testProjectOutputPath = "bin/debug";
            var coverageSession = new CoverageSession();

            dotNetTestRunner.Run(coverageSession, testProjectOutputPath);

            Received.InOrder(() =>
            {
                _rpcServerMock.Start(Arg.Any<CoverageSession>());
                _process.Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            });
        }

        [Fact]
        public void Execute_DotNet_Test_Command_Line()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var coverageSession = new CoverageSession();
            var testProjectOutputPath = "bin/debug";

            dotNetTestRunner.Run(coverageSession, testProjectOutputPath);

            _process.Received(1).Execute(Arg.Is("dotnet"), Arg.Is("test --no-build"), Arg.Any<string>());
        }

        [Fact]
        public void Use_Project_Folder_As_Working_Directory()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var testProjectOutputPath = @"c:\git\project\src\projectname\bin\debug\netcoreapp1.1\";
            var expectedProjectName = @"c:\git\project\src\projectname";
            var coverageSession = new CoverageSession();

            dotNetTestRunner.Run(coverageSession, testProjectOutputPath);

            _process.Received(1).Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Is(expectedProjectName));
        }

        [Fact]
        public void Stop_Rpc_Server_After_Running_Tests()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var testProjectOutputPath = "bin /debug";
            var coverageSession = new CoverageSession();

            dotNetTestRunner.Run(coverageSession, testProjectOutputPath);

            Received.InOrder(() =>
            {
                _process.Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                _rpcServerMock.Stop();
            });
        }
    }
}
