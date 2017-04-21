// MIT License
// Copyright (c) 2017 Paulo Gomes

using System.Linq;
using CoreCover.Framework.Abstractions;

namespace CoreCover.Framework
{
    public class ConsoleRunner
    {
        private readonly IConsole _console;
        private readonly ICoverageRunner _coverageRunner;

        public ConsoleRunner(IConsole console, ICoverageRunner coverageRunner)
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
            
            var testProjectOutputPath = inputArgs.First();
            var reportPath = inputArgs.Last();
            
            _coverageRunner.Run(testProjectOutputPath, reportPath);
        }
    }
}