using System;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace CoreCover
{
    public sealed class InstrumentatorHandler : AssemblyHandler
    {
        public InstrumentatorHandler() : this(null)
        {
            
        }

        public InstrumentatorHandler(AssemblyHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Instrumentating Assembly: {assemblyDefinition.FullName}");
            foreach (var module in assemblyDefinition.Modules)
            {
                ProcessModule(module);
            }

            base.Handle(assemblyDefinition);
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
                for (var i = ilProcessor.Body.Instructions.Count; i > 0; i--)
                {
                    var instruction = ilProcessor.Body.Instructions[i - 1];
                    var sequencePoint = method.DebugInformation.GetSequencePoint(instruction);
                    if (sequencePoint != null)
                    {
                        ilProcessor.Body.SimplifyMacros();
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Call, (MethodReference)instrumentationMethodRef));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldc_I4, sequencePoint.StartLine));
                        ilProcessor.InsertAfter(instruction,
                            Instruction.Create(OpCodes.Ldstr, sequencePoint.Document.Url));
                        ilProcessor.Body.OptimizeMacros();
                    }
                }
            }
        }

        private MethodReference GetMethodReference(ModuleDefinition module)
        {
            var voidRef = module.ImportReference(
                new TypeReference("System", "Void", null, new AssemblyNameReference("netstandard", null)));
            var coverageTrackerRef = module.ImportReference(
                new TypeReference("CoreCover.Instrumentation", "CoverageTracker", null,
                    new AssemblyNameReference("CoreCover.Instrumentation", null)));
            var instrumentationMethodRef = module.ImportReference(new MethodReference("MarkExecution", voidRef,
                coverageTrackerRef));
            var stringRef = module.ImportReference(
                new TypeReference("System", "String", null, new AssemblyNameReference("netstandard", null)));
            var int32Ref = module.ImportReference(
                new TypeReference("System", "Int32", null, new AssemblyNameReference("netstandard", null)));

            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("fileName", ParameterAttributes.In, stringRef));
            instrumentationMethodRef.Parameters.Add(
                new ParameterDefinition("lineNumber", ParameterAttributes.In, int32Ref));
            return instrumentationMethodRef;
        }
    }
}