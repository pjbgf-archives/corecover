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
        private readonly ILogger _consoleMock;
        private readonly ICoverageRunner _coverageRunnerMock;

        public ConsoleRunnerShould()
        {
            _consoleMock = Substitute.For<ILogger>();
            _coverageRunnerMock = Substitute.For<ICoverageRunner>();
        }

        [Theory]
        [InlineData(null)]
        public void Show_Usage_When_Not_All_Mandatory_Parameters_Are_Provided(params string[] inputArgs)
        {
            var consoleRunner = new ConsoleRunner(_consoleMock, _coverageRunnerMock);

            consoleRunner.ProcessCommand(inputArgs);

            _consoleMock.ReceivedWithAnyArgs(1).LogError(Arg.Any<string>());
            _coverageRunnerMock.DidNotReceiveWithAnyArgs().Run(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Run_Coverage_Runner_When_All_Mandatory_Parameters_Are_Provided()
        {
            var consoleRunner = new ConsoleRunner(_consoleMock, _coverageRunnerMock);

            consoleRunner.ProcessCommand("TestProject//OutputPath");

            _consoleMock.DidNotReceiveWithAnyArgs().LogError(Arg.Any<string>());
            _coverageRunnerMock.ReceivedWithAnyArgs(1).Run(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
