// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using Grpc.Core;

namespace CoreCover.Instrumentation
{
    public static class CoverageTracker
    {
        private static readonly ExecutionTracker.ExecutionTrackerClient ExecutionTrackerClient;
        private static string ServerAddress = "127.0.0.1";
        private static string ServerPort = "50051";

        static CoverageTracker()
        {
            var channel = new Channel($"{ServerAddress}:{ServerPort}", ChannelCredentials.Insecure);
            ExecutionTrackerClient = new ExecutionTracker.ExecutionTrackerClient(channel);
        }

        public static void MarkExecution(string moduleHash, int metadataToken, int startLineNumber, int endLineNumber)
        {
            ExecutionTrackerClient.Track(new ExecutedLine { ModuleHash = moduleHash, MetadataToken = metadataToken, StartLineNumber = startLineNumber, EndLineNumber = endLineNumber});
        }
    }
}
