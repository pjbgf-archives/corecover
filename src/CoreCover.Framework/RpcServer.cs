using CoreCover.Framework.Abstractions;
using CoreCover.Instrumentation;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class RpcServer : IRpcServer
    {
        private readonly ILogger _logger;
        private readonly Server _server;
        private static readonly string ServerAddress = "localhost";
        private static readonly int ServerPort = 50051;

        public RpcServer(ILogger logger)
        {
            _logger = logger;
            _server = new Server();
            _server.Ports.Add(new ServerPort(ServerAddress, ServerPort, ServerCredentials.Insecure));
        }

        public void Start(CoverageSession coverageSession)
        {
            _logger.LogInformation($"Starting RPC server at {ServerAddress}:{ServerPort}.");
            _server.Services.Add(ExecutionTracker.BindService(new ExecutionTrackerServer(coverageSession)));
            _server.Start();
        }

        public void Stop()
        {
            _logger.LogInformation("Stopping RPC server.");
            _server.ShutdownAsync().Wait();
        }
    }
}