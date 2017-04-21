using System;
using System.Collections;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public sealed class CodeInstrumentationHandler : AssemblyInstrumentationHandler
    {
        public CodeInstrumentationHandler() : this((AssemblyInstrumentationHandler)null)
        {

        }

        public CodeInstrumentationHandler(AssemblyInstrumentationHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Instrumentating Assembly: {assemblyDefinition.FullName}");
            foreach (var module in assemblyDefinition.Modules)
            {
                ProcessModule(module);
            }

            base.Handle(coverageSession, assemblyDefinition);
        }

        private void ProcessModule(ModuleDefinition module)
        {
            Console.WriteLine($"Module: {module.Name}");

            foreach (var type in module.Types)
            {
                ProcessType(type);
            }
        }

        private void ProcessType(TypeDefinition type)
        {
            Console.WriteLine($"Type: {type.Name}");

            foreach (var method in type.Methods)
            {
                ProcessMethod(method);
            }
        }

        private void ProcessMethod(MethodDefinition method)
        {
            Console.WriteLine($"Method: {method.Name}");

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