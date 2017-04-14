using System.Linq;

namespace CoreCover.Framework
{
    public class ConsoleRunner
    {
        private readonly IConsole _console;
        private readonly IRunner _coverageRunner;

        public ConsoleRunner(IConsole console, IRunner coverageRunner)
        {
            _console = console;
            _coverageRunner = coverageRunner;
        }

        public void ProcessCommand(params string[] inputArgs)
        {
            if (inputArgs == null || inputArgs.Length < 2)
            {
                _console.WriteLine("usage: dotnet corecover.dll TestProjectOutputPath [path]coverage-report.xml");
                return;
            }

            //TODO: Load target dlls through csproj
            var testProjectOutputPath = inputArgs.First();
            var reportPath = inputArgs.Last();
            
            _coverageRunner.Run(testProjectOutputPath, reportPath);
        }
    }
}