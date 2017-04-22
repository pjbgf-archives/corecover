// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Linq;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public sealed class CodeInstrumentationHandler : AssemblyInstrumentationHandler
    {
        private const string InstrumentedAssemblyFlagName = "__CORECOVER__";
        private readonly ILogger _logger;

        public CodeInstrumentationHandler(ILogger logger) : this(logger, null)
        {
        }

        public CodeInstrumentationHandler(ILogger logger, AssemblyInstrumentationHandler sucessorHandler) : base(sucessorHandler)
        {
            _logger = logger;
        }

        public override void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition)
        {
            _logger.LogInformation($"Instrumentating Assembly: {assemblyDefinition.FullName}");
            foreach (var module in assemblyDefinition.Modules)
            {
                if (!IsAssemblyInstrumented(assemblyDefinition))
                {
                    ProcessModule(module);
                    MarkAssemblyAsInstrumented(assemblyDefinition);
                }
                else
                    _logger.LogInformation($"Skipping {assemblyDefinition.FullName}, assembly is already instrumented.");
            }

            base.Handle(coverageSession, assemblyDefinition);
        }

        private static bool IsAssemblyInstrumented(AssemblyDefinition assembly)
        {
            return assembly.Modules.First().Types.Any(x => x.Name == InstrumentedAssemblyFlagName);
        }

        private static void MarkAssemblyAsInstrumented(AssemblyDefinition assemblyDefinition)
        {
            assemblyDefinition.Modules.First().Types.Add(new TypeDefinition("CoreCover", InstrumentedAssemblyFlagName, TypeAttributes.NotPublic));
        }

        private void ProcessModule(ModuleDefinition module)
        {
            _logger.LogInformation($"Module: {module.Name}");

            foreach (var type in module.Types)
            {
                ProcessType(type);
            }
        }

        private void ProcessType(TypeDefinition type)
        {
            _logger.LogInformation($"Type: {type.Name}");

            foreach (var method in type.Methods)
            {
                ProcessMethod(method);
            }
        }

        private void ProcessMethod(MethodDefinition method)
        {
            _logger.LogInformation($"Method: {method.Name}");

            var ilProcessor = method.Body.GetILProcessor();
            var instrumentationMethodRef = GetMethodReference(method.Module);

            // More info around portable PDB and debug information can be found at: 
            // https://github.com/dotnet/corefx/blob/master/src/System.Reflection.Metadata/specs/PortablePdb-Metadata.md
            if (method.DebugInformation.HasSequencePoints)
            {
                ilProcessor.Body.SimplifyMacros();

                for (var i = ilProcessor.Body.Instructions.Count; i > 0; i--)
                {
                    var instruction = ilProcessor.Body.Instructions[i - 1];
                    if (instruction.OpCode != OpCodes.Nop)
                        continue;

                    var sequencePoint = method.DebugInformation.GetSequencePoint(instruction);
                    if (sequencePoint != null)
                    {
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Call, (MethodReference)instrumentationMethodRef));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldc_I4, sequencePoint.EndLine));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldc_I4, sequencePoint.StartLine));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldc_I4, method.MetadataToken.ToInt32()));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldstr, method.Module.Mvid.ToString()));
                    }
                }

                ilProcessor.Body.OptimizeMacros();
            }
        }

        private MethodReference GetMethodReference(ModuleDefinition module)
        {
            var voidRef = module.ImportReference(
                new TypeReference("System", "Void", null, new AssemblyNameReference("netstandard", null)));
            var coverageTrackerRef = module.ImportReference(
                new TypeReference("CoreCover.Instrumentation", "CoverageTracker", null,
                    new AssemblyNameReference("CoreCover.Instrumentation", new Version(1, 0))));
            var instrumentationMethodRef = module.ImportReference(new MethodReference("MarkExecution", voidRef,
                coverageTrackerRef));
            var stringRef = module.ImportReference(
                new TypeReference("System", "String", null, new AssemblyNameReference("netstandard", null)));
            var int32Ref = module.ImportReference(
                new TypeReference("System", "Int32", null, new AssemblyNameReference("netstandard", null)));

            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("moduleHash", ParameterAttributes.In, stringRef));
            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("metadataToken", ParameterAttributes.In, int32Ref));
            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("startLineNumber", ParameterAttributes.In, int32Ref));
            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("endLineNumber", ParameterAttributes.In, int32Ref));

            return instrumentationMethodRef;
        }
    }
}