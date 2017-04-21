// MIT License
// Copyright (c) 2017 Paulo Gomes

using CoreCover.Framework;
using CoreCover.Framework.Adapters;

namespace CoreCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var console = new ConsoleAdapter();

            new ConsoleRunner(
                console, new CoverageRunner(
                    new Instrumentator(new CodeCoverageHandler(new CodeInstrumentationHandler(console))),
                    new DotNetTestRunner(), 
                    new OpenCoverReportAdapter(), new RpcServer()))
                .ProcessCommand(args);
        }
    }
}