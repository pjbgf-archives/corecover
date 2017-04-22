// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

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
            if (inputArgs == null || inputArgs.Length < 1)
            {
                _logger.LogCritical("usage: dotnet corecover.dll TestProjectOutputPath [path]coverage-report.xml");
                return;
            }
            
            var testProjectOutputPath = inputArgs.First();
            var reportPath = inputArgs.ElementAtOrDefault(1);
            
            _coverageRunner.Run(testProjectOutputPath, reportPath);
        }
    }
}