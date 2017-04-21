using CoreCover.Framework.Abstractions;
using CoreCover.Instrumentation;
using Grpc.Core;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class RpcServer : IRpcServer
    {
        private readonly Server _server;
        private static readonly string ServerAddress = "localhost";
        private static readonly int ServerPort = 50051;

        public RpcServer()
        {
            _server = new Server();
            _server.Ports.Add(new ServerPort(ServerAddress, ServerPort, ServerCredentials.Insecure));
        }

        public void Start(CoverageSession coverageSession)
        {
            _server.Services.Add(ExecutionTracker.BindService(new ExecutionTrackerServer(coverageSession)));
            _server.Start();
        }

        public void Stop()
        {
            _server.ShutdownAsync().Wait();
        }
    }
}