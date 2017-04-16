using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CoreCover.Instrumentation;
using Mono.Cecil;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public sealed class CodeCoverageHandler : AssemblyInstrumentationHandler
    {
        public CodeCoverageHandler(AssemblyInstrumentationHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Reporting Assembly: {assemblyDefinition.FullName}");

            var modules = new List<Module>(coverageSession.Modules.Length + assemblyDefinition.Modules.Count);
            modules.AddRange(coverageSession.Modules);

            foreach (var module in assemblyDefinition.Modules)
            {
                modules.Add(ProcessModule(coverageSession, module));
            }

            coverageSession.Modules = modules.ToArray();

            base.Handle(coverageSession, assemblyDefinition);
        }

        private Module ProcessModule(CoverageSession coverageSession, ModuleDefinition module)
        {
            var coverageModule = new Module();
            var types = new List<Class>(module.Types.Count);

            foreach (var type in module.Types)
                types.Add(ProcessType(type));

            coverageModule.Classes = types.ToArray();
            return coverageModule;
        }

        private Class ProcessType(TypeDefinition type)
        {
            var coverageClass = new Class();
            var methods = new List<Method>(type.Methods.Capacity);

            coverageClass.FullName = type.FullName;

            foreach (var method in type.Methods)
                methods.Add(ProcessMethod(method));

            return coverageClass;
        }

        private Method ProcessMethod(MethodDefinition method)
        {
            var coverageMethod = new Method();

            coverageMethod.FullName = method.FullName;
            coverageMethod.IsConstructor = method.IsConstructor;
            coverageMethod.IsStatic = method.IsStatic;
            coverageMethod.IsSetter = method.IsSetter;
            coverageMethod.IsGetter = method.IsGetter;

            if (method.DebugInformation.HasSequencePoints)
            {
                coverageMethod.SequencePoints = method.DebugInformation.SequencePoints.Select(x => new SequencePoint
                {
                    Offset = x.Offset,
                    StartLine = x.StartLine,
                    EndLine = x.EndLine,
                    StartColumn = x.StartColumn,
                    EndColumn = x.EndColumn
                }).ToArray();

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

            return coverageMethod;
        }
    }
}