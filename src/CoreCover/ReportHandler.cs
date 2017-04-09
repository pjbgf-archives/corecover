using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using CoreCover.Instrumentation;
using Mono.Cecil;

namespace CoreCover
{
    public sealed class ReportHandler : AssemblyHandler
    {
        public ReportHandler(AssemblyHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Reporting Assembly: {assemblyDefinition.FullName}");
            foreach (var module in assemblyDefinition.Modules)
                ProcessModule(module);

            base.Handle(assemblyDefinition);
        }

        private void ProcessModule(ModuleDefinition module)
        {
            foreach (var type in module.Types)
                ProcessType(type);
        }

        private void ProcessType(TypeDefinition type)
        {
            foreach (var method in type.Methods)
                ProcessMethod(method);
        }

        private void ProcessMethod(MethodDefinition method)
        {
            if (method.DebugInformation.HasSequencePoints)
            {
                var ilProcessor = method.Body.GetILProcessor();
                for (var i = ilProcessor.Body.Instructions.Count; i > 0; i--)
                {
                    var instruction = ilProcessor.Body.Instructions[i - 1];
                    var sequencePoint = method.DebugInformation.GetSequencePoint(instruction);
                    if (sequencePoint != null)
                    {
                        ReportTracker.ReportLine(sequencePoint.Document.Url, sequencePoint.StartLine);
                    }
                }
            }
        }
    }
}