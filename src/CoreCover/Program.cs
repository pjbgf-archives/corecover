// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.IO;
using System.Reflection;
using CoreCover.Framework;
using CoreCover.Framework.Adapters;
using CoreCover.Framework.CodeAnalysis;
using CoreCover.Framework.TestExecution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = GetLogger();
            new ConsoleRunner(
                    new ConsoleAdapter(), new CoverageRunner(
                    new Instrumentator(logger, new CodeCoverageHandler(new CodeInstrumentationHandler(logger))),
                    new DotNetTestRunner(new RpcServer(logger), new Process()), 
                    new OpenCoverReportAdapter()))
                .ProcessCommand(args);
        }

        private static ILogger<Program> GetLogger()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
                .AddJsonFile("appSettings.json", false);

            var config = builder.Build();
            var loggerFactory = new LoggerFactory()
                .AddConsole(config.GetSection("Logging"));

            var logger = loggerFactory.CreateLogger<Program>();
            return logger;
        }
    }
}