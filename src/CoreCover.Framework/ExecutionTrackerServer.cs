// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCover.Instrumentation;
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
            var method = _coverageSession.Modules.First(x => x.ModuleHash == request.ModuleHash)
                .Classes.First(x => x.Methods.Any(y => y.MetadataToken == request.MetadataToken))
                .Methods.First(y => y.MetadataToken == request.MetadataToken);

            method.Visited = true;
            var sequencePoints = method.SequencePoints.Where(x => request.StartLineNumber == x.StartLine);

            foreach (var sequencePoint in sequencePoints)
                sequencePoint.VisitCount++;
            
            Console.WriteLine($"Server: {request.MetadataToken}:{request.StartLineNumber}:{request.EndLineNumber}");
            return Task.FromResult(new ExecutedLineReply());
        }
    }
}
