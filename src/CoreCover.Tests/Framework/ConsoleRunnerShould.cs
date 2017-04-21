// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework;
using CoreCover.Framework.Abstractions;
using NSubstitute;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class ConsoleRunnerShould
    {
        [Theory]
        [InlineData(null)]
        [InlineData("ProjectPath")]
        public void Show_Usage_When_Not_All_Mandatory_Parameters_Are_Provided(params string[] inputArgs)
        {
            var consoleMock = Substitute.For<IConsole>();
            var codeCoverageMock = Substitute.For<ICoverageRunner>();

            var consoleRunner = new ConsoleRunner(consoleMock, codeCoverageMock);

            consoleRunner.ProcessCommand(inputArgs);

            consoleMock.ReceivedWithAnyArgs(1).WriteLine(Arg.Any<string>());
            codeCoverageMock.DidNotReceiveWithAnyArgs().Run(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Run_Coverage_Tool_When_All_Mandatory_Parameters_Are_Provided()
        {
            var consoleMock = Substitute.For<IConsole>();
            var codeCoverageMock = Substitute.For<ICoverageRunner>();

            var consoleRunner = new ConsoleRunner(consoleMock, codeCoverageMock);

            consoleRunner.ProcessCommand("TestProject//OutputPath", "coverage-report.xml");

            consoleMock.DidNotReceiveWithAnyArgs().WriteLine(Arg.Any<string>());
            codeCoverageMock.ReceivedWithAnyArgs(1).Run(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
