// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCover.Framework.Model;
using CoreCover.Instrumentation;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public class ExecutionTrackerServer : ExecutionTracker.ExecutionTrackerBase
    {
        private readonly ILogger _logger;
        private readonly CoverageContext _coverageContext;

        public ExecutionTrackerServer(CoverageContext coverageContext, ILogger logger = null)
        {
            _logger = logger;
            _coverageContext = coverageContext;
        }

        public override Task<ExecutedLineReply> Track(ExecutedLine request, ServerCallContext context)
        {
            _logger?.LogInformation($"gRPC Server Received: {request.MetadataToken}:{request.StartLineNumber}:{request.EndLineNumber}");

            _coverageContext.MarkExecution(request);
            
            return Task.FromResult(new ExecutedLineReply());
        }
    }
}
