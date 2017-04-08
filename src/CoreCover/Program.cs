using System;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;

namespace CoreCover
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new[]
                {@"C:\git\CoreCover\src\CoreCover.Sample.Library\bin\Debug\netcoreapp1.1\CoreCover.Sample.Library.dll"};

            if (args == null || args.Length == 0)
            {
                Console.WriteLine("usage: CoreCover [path]fileName.dll");
                return;
            }

            foreach (var assemblyPath in args)
            {
                var assembly = LoadAssembly(assemblyPath);
                ProcessAssembly(assembly);
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

            //var instrumentationCall = 

            //var ilProcessor = method.Body.GetILProcessor();
            //var firstInstruction = ilProcessor.Body.Instructions[0];
            //ilProcessor.InsertAfter(firstInstruction, instrumentationCall);
        }
    }
}