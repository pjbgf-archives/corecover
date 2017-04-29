// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCover.Instrumentation;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class ExecutionTrackerServer : ExecutionTracker.ExecutionTrackerBase
    {
        private readonly ILogger _logger;
        private readonly CoverageSession _coverageSession;

        public ExecutionTrackerServer(CoverageSession coverageSession, ILogger logger = null)
        {
            _logger = logger;
            _coverageSession = coverageSession;
        }

        public override Task<ExecutedLineReply> Track(ExecutedLine request, ServerCallContext context)
        {
            _logger?.LogInformation($"gRPC Server Received: {request.MetadataToken}:{request.StartLineNumber}:{request.EndLineNumber}");
            
            //TODO: Requires refactoring
            var module = _coverageSession.Modules.First(x => x.ModuleHash == request.ModuleHash);
            var @class = module
                .Classes.First(x => x.Methods.Any(y => y.MetadataToken == request.MetadataToken));
            var method = @class
                .Methods.First(y => y.MetadataToken == request.MetadataToken);

            method.Visited = true;
            var sequencePoints = method.SequencePoints.Where(x => request.StartLineNumber == x.StartLine);

            foreach (var sequencePoint in sequencePoints)
                sequencePoint.VisitCount++;
            
            return Task.FromResult(new ExecutedLineReply());
        }
    }
}
