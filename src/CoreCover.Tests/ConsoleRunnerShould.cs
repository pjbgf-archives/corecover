using NSubstitute;
using Xunit;

namespace CoreCover.Tests
{
    public class ConsoleRunnerShould
    {
        [Fact]
        public void Show_Usage_When_One_Parameters_Is_Provided()
        {
            var consoleMock = Substitute.For<IConsole>();
            var coverageToolMock = Substitute.For<IRunner>();

            var consoleRunner = new ConsoleRunner(consoleMock, coverageToolMock);

            consoleRunner.ProcessCommand(new string [] { "" });

            consoleMock.ReceivedWithAnyArgs(1).WriteLine(Arg.Any<string>());
            coverageToolMock.DidNotReceiveWithAnyArgs().Run(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
