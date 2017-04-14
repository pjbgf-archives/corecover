using NSubstitute;
using Xunit;

namespace CoreCover.Tests
{
    public class ConsoleRunnerShould
    {
        [Theory]
        [InlineData(null)]
        [InlineData("ProjectPath")]
        public void Show_Usage_When_Not_All_Mandatory_Parameters_Are_Provided(params string[] inputArgs)
        {
            var consoleMock = Substitute.For<IConsole>();
            var coverageToolMock = Substitute.For<IRunner>();

            var consoleRunner = new ConsoleRunner(consoleMock, coverageToolMock);

            consoleRunner.ProcessCommand(inputArgs);

            consoleMock.ReceivedWithAnyArgs(1).WriteLine(Arg.Any<string>());
            coverageToolMock.DidNotReceiveWithAnyArgs().Run(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Run_Coverage_Tool_When_All_Mandatory_Parameters_Are_Provided()
        {
            var consoleMock = Substitute.For<IConsole>();
            var coverageToolMock = Substitute.For<IRunner>();

            var consoleRunner = new ConsoleRunner(consoleMock, coverageToolMock);

            consoleRunner.ProcessCommand("TestProject//OutputPath", "coverage-report.xml");

            consoleMock.DidNotReceiveWithAnyArgs().WriteLine(Arg.Any<string>());
            coverageToolMock.ReceivedWithAnyArgs(1).Run(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
