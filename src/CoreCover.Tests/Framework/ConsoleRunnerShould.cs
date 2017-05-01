// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class ConsoleRunnerShould
    {
        private readonly IConsole _consoleMock;
        private readonly ICoverageRunner _coverageRunnerMock;

        public ConsoleRunnerShould()
        {
            _consoleMock = Substitute.For<IConsole>();
            _coverageRunnerMock = Substitute.For<ICoverageRunner>();
        }

        [Theory]
        [InlineData(null)]
        public void Show_Usage_When_Not_All_Mandatory_Parameters_Are_Provided(params string[] inputArgs)
        {
            var consoleRunner = new ConsoleRunner(_consoleMock, _coverageRunnerMock);

            consoleRunner.ProcessCommand(inputArgs);

            _consoleMock.Received(1).WriteLine(Arg.Any<string>());
            _coverageRunnerMock.DidNotReceive().Run(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Run_Coverage_Runner_When_All_Mandatory_Parameters_Are_Provided()
        {
            var consoleRunner = new ConsoleRunner(_consoleMock, _coverageRunnerMock);

            consoleRunner.ProcessCommand("TestProject//OutputPath");

            _consoleMock.DidNotReceive().WriteLine(Arg.Any<string>());
            _coverageRunnerMock.Received(1).Run(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Sanatise_Input_To_Remove_Quotes()
        {
            var consoleRunner = new ConsoleRunner(_consoleMock, _coverageRunnerMock);

            consoleRunner.ProcessCommand("\"TestProject//OutputPath\"", "\"report.xml\"");

            _consoleMock.DidNotReceive().WriteLine(Arg.Any<string>());
            _coverageRunnerMock.Received(1).Run(Arg.Is("TestProject//OutputPath"), Arg.Is("report.xml"));
        }
    }
}
