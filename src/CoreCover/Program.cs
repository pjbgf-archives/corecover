// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework;
using CoreCover.Framework.Adapters;
using Microsoft.Extensions.Logging;

namespace CoreCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = new LoggerFactory().AddConsole().CreateLogger("");
            var console = new ConsoleAdapter();

            new ConsoleRunner(
                    logger, new CoverageRunner(
                    new Instrumentator(new CodeCoverageHandler(new CodeInstrumentationHandler(logger))),
                    new DotNetTestRunner(new RpcServer(), new Process()), 
                    new OpenCoverReportAdapter()))
                .ProcessCommand(args);
        }
    }
}