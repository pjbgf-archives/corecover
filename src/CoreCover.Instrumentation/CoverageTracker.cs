using System;
using CoreCover.Framework;
using Grpc.Core;

namespace CoreCover.Instrumentation
{
    public static class CoverageTracker
    {
        private static readonly ExecutionTracker.ExecutionTrackerClient ExecutionTrackerClient;

        static CoverageTracker()
        {
            var channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            ExecutionTrackerClient = new ExecutionTracker.ExecutionTrackerClient(channel);
        }

        public static void MarkExecution(string moduleHash, int metadataToken, int startLineNumber, int endLineNumber)
        {
            ExecutionTrackerClient.Track(new ExecutedLine { ModuleHash = moduleHash, MetadataToken = metadataToken, StartLineNumber = startLineNumber, EndLineNumber = endLineNumber});
        }
    }
}
