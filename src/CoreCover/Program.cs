using CoreCover.Framework;
using CoreCover.Framework.Adapters;

namespace CoreCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ConsoleRunner(
                new ConsoleWrapper(), new CodeCoverage(
                    new Instrumentator(new ReportHandler(new InstrumentatorHandler())), 
                    new DotNetTestRunner(), 
                    new OpenCoverReportAdapter()))
                .ProcessCommand(args);
        }
    }
}