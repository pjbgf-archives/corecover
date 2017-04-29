// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.Linq;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;

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
            if (inputArgs == null || inputArgs.Length < 1)
            {
                _console.WriteLine("usage: dotnet corecover.dll TestProjectOutputPath [path]coverage-report.xml");
                return;
            }
            
            var testProjectOutputPath = SanitiseInput(inputArgs.First());
            var reportPath = SanitiseInput(inputArgs.ElementAtOrDefault(1));
            
            _coverageRunner.Run(testProjectOutputPath, reportPath);
        }

        string SanitiseInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input.Replace("\"", string.Empty);
        }
    }
}