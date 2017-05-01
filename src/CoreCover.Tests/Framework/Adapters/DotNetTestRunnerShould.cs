// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.IO;
using System.Runtime.InteropServices;
using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Adapters;
using CoreCover.Framework.Model;
using NSubstitute;
using OpenCover.Framework.Model;
using Xunit;

namespace CoreCover.Tests.Framework.Adapters
{
    public class DotNetTestRunnerShould
    {
        private readonly IProcess _process;
        private readonly IRpcServer _rpcServerMock;

        private static readonly bool IsWindows = System.Runtime.InteropServices.RuntimeInformation
            .IsOSPlatform(OSPlatform.Windows);

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
            var coverageContext = new CoverageContext();

            dotNetTestRunner.Run(coverageContext, testProjectOutputPath);

            Received.InOrder(() =>
            {
                _rpcServerMock.Start(Arg.Any<CoverageContext>());
                _process.Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            });
        }

        [Fact]
        public void Execute_DotNet_Test_Command_Line()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var coverageContext = new CoverageContext();
            var testProjectOutputPath = "bin/debug";

            dotNetTestRunner.Run(coverageContext, testProjectOutputPath);

            _process.Received(1).Execute(Arg.Is("dotnet"), Arg.Is("test --no-build"), Arg.Any<string>());
        }

        [Fact(Skip = "Need to refactor DotNetTestRunner to improve its testability")]
        public void Use_Project_Folder_As_Working_Directory()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var coverageContext = new CoverageContext();
            string testProjectOutputPath;
            string expectedProjectName;

            if (IsWindows)
            {
                testProjectOutputPath = @"c:\git\project\src\projectname\bin\debug\netcoreapp1.1\";
                expectedProjectName = @"c:\git\project\src\projectname";
            }
            else
            {
                testProjectOutputPath = @"/home/user/git/project/src/projectname/bin/debug/netcoreapp1.1/";
                expectedProjectName = @"/home/user/git/project/src/projectname";
            }

            dotNetTestRunner.Run(coverageContext, testProjectOutputPath);

            _process.Received(1).Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Is(expectedProjectName));
        }

        [Fact(Skip = "Need to refactor DotNetTestRunner to improve its testability")]
        public void Disregard_Lack_Of_Trailing_slash()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var coverageContext = new CoverageContext();
            string testProjectOutputPath;
            string expectedProjectName;

            if (IsWindows)
            {
                testProjectOutputPath = @"c:\git\project\src\projectname\bin\debug\netcoreapp1.1";
                expectedProjectName = @"c:\git\project\src\projectname";
            }
            else
            {
                testProjectOutputPath = @"/home/user/git/project/src/projectname/bin/debug/netcoreapp1.1";
                expectedProjectName = @"/home/user/git/project/src/projectname";
            }

            dotNetTestRunner.Run(coverageContext, testProjectOutputPath);

            _process.Received(1).Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Is(expectedProjectName));
        }

        [Fact]
        public void Stop_Rpc_Server_After_Running_Tests()
        {
            var dotNetTestRunner = new DotNetTestRunner(_rpcServerMock, _process);
            var testProjectOutputPath = "bin/debug";
            var coverageContext = new CoverageContext();

            dotNetTestRunner.Run(coverageContext, testProjectOutputPath);

            Received.InOrder(() =>
            {
                _process.Execute(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                _rpcServerMock.Stop();
            });
        }
    }
}
