using System;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Mono.Cecil.Cil;

namespace CoreCover
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[]
                {@"C:\git\corecover\src\CoreCover.Sample.Tests\bin\Debug\netcoreapp1.1\CoreCover.Sample.Library.dll"};

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("usage: CoreCover [path]fileName.dll");
                return;
            }

            foreach (var assemblyPath in args)
            {
                if (File.Exists(assemblyPath + ".bak"))
                    File.Delete(assemblyPath + ".bak");

                File.Move(assemblyPath, assemblyPath + ".bak");
                var assembly = LoadAssembly(assemblyPath + ".bak");
                ProcessAssembly(assembly);
                assembly.Write(assemblyPath);
            }
        }

        private static AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);

            Console.WriteLine($"Processing Assembly: {assembly.FullName}");

            return assembly;
        }

        private static void ProcessAssembly(AssemblyDefinition assembly)
        {
            foreach (var module in assembly.Modules)
            {
                ProcessModule(module);
            }
        }

        private static void ProcessModule(ModuleDefinition module)
        {
            Console.WriteLine($"Module: {module.Name}");

            foreach (var type in module.Types)
            {
                ProcessType(type);
            }
        }

        private static void ProcessType(TypeDefinition type)
        {
            Console.WriteLine($"Type: {type.Name}");

            foreach (var method in type.Methods)
            {
                ProcessMethod(method);
            }
        }

        private static void ProcessMethod(MethodDefinition method)
        {
            Console.WriteLine($"Method: {method.Name}");

            var ilProcessor = method.Body.GetILProcessor();
            var firstInstruction = ilProcessor.Body.Instructions[0];

            ilProcessor.Body.SimplifyMacros();

            var instrumentationMethodRef = GetMethodReference(method.Module);
            ilProcessor.InsertAfter(firstInstruction, Instruction.Create(OpCodes.Call, instrumentationMethodRef));
            ilProcessor.InsertAfter(firstInstruction, Instruction.Create(OpCodes.Ldc_I4, 66));
            ilProcessor.InsertAfter(firstInstruction, Instruction.Create(OpCodes.Ldstr, "FileName"));

            ilProcessor.Body.OptimizeMacros();
        }

        private static MethodReference GetMethodReference(ModuleDefinition module)
        {
            var voidRef = module.ImportReference(
                new TypeReference("System", "Void", null, new AssemblyNameReference("netstandard", null)));
            var coverageTrackerRef = module.ImportReference(
                new TypeReference("CoreCover.Instrumentation", "CoverageTracker", null,
                    new AssemblyNameReference("CoreCover.Instrumentation", Version.Parse("1.0.0.0"))));
            var instrumentationMethodRef = module.ImportReference(new MethodReference("MarkExecution", voidRef,
                coverageTrackerRef));
            var stringRef = module.ImportReference(
                new TypeReference("System", "String", null, new AssemblyNameReference("netstandard", null)));
            var int32Ref = module.ImportReference(
                new TypeReference("System", "Int32", null, new AssemblyNameReference("netstandard", null)));

            instrumentationMethodRef.Parameters.Add(new ParameterDefinition("fileName", ParameterAttributes.In, stringRef));
            instrumentationMethodRef.Parameters.Add(new ParameterDefinition("lineNumber", ParameterAttributes.In, int32Ref));
            return instrumentationMethodRef;
        }
    }
}