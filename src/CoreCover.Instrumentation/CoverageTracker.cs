using System;
using CoreCover.Framework;
using Grpc.Core;

namespace CoreCover.Instrumentation
{
    public static class CoverageTracker
    {
        private static readonly ExecutionTracker.ExecutionTrackerClient _executionTrackerClient;

        static CoverageTracker()
        {
            var channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            _executionTrackerClient = new ExecutionTracker.ExecutionTrackerClient(channel);
        }

        public static void MarkExecution(string moduleHash, int metadataToken, int lineNumber)
        {
            _executionTrackerClient.Track(new ExecutedLine { ModuleHash = moduleHash, MetadataToken = metadataToken, LineNumber = lineNumber });
        }
    }
}
