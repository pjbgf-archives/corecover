// MIT License
// Copyright (c) 2017 Paulo Gomes

using System.Linq;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;

namespace CoreCover.Framework
{
    public class ConsoleRunner
    {
        private readonly ILogger _logger;
        private readonly ICoverageRunner _coverageRunner;

        public ConsoleRunner(ILogger logger, ICoverageRunner coverageRunner)
        {
            _logger = logger;
            _coverageRunner = coverageRunner;
        }

        public void ProcessCommand(params string[] inputArgs)
        {
            if (inputArgs == null || inputArgs.Length < 2)
            {
                _logger.LogError("usage: dotnet corecover.dll TestProjectOutputPath [path]coverage-report.xml");
                return;
            }
            
            var testProjectOutputPath = inputArgs.First();
            var reportPath = inputArgs.Last();
            
            _coverageRunner.Run(testProjectOutputPath, reportPath);
        }
    }
}