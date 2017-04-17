using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace CoreCover.Framework
{
    public class ExecutionTrackerServer : ExecutionTracker.ExecutionTrackerBase
    {
        public override Task<ExecutedLineReply> Track(ExecutedLine request, ServerCallContext context)
        {
            Console.WriteLine($"Server: {request.FileName}:{request.LineNumber}");
            return Task.FromResult(new ExecutedLineReply());
        }
    }
}
