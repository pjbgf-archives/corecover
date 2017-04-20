using System;
using System.Threading.Tasks;
using Grpc.Core;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class ExecutionTrackerServer : ExecutionTracker.ExecutionTrackerBase
    {
        private readonly CoverageSession _coverageSession;

        public ExecutionTrackerServer(CoverageSession coverageSession)
        {
            _coverageSession = coverageSession;
        }

        public override Task<ExecutedLineReply> Track(ExecutedLine request, ServerCallContext context)
        {
            Console.WriteLine($"Server: {request.FileName}:{request.LineNumber}");
            return Task.FromResult(new ExecutedLineReply());
        }
    }
}
