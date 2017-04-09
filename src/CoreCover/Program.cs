using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CoreCover.Instrumentation;
using Mono.Cecil;
using Mono.Cecil.Pdb;

namespace CoreCover
{
    partial class Program
    {
        static void Main(string[] args)
        {
            args = new[]
                {@"C:\git\corecover\src\CoreCover.Sample.Tests\bin\Debug\netcoreapp1.1\CoreCover.Sample.Library.dll"};

            //TODO: Try to load target dlls through csproj
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("usage: CoreCover [path]MyTests.dll");
                return;
            }
            
            CopyInstrumentationToTargetPath(Path.GetDirectoryName(args[0]));
            new Program().ProcessAssemblies(args);
            ReportTracker.WriteReport();
        }

        private static void CopyInstrumentationToTargetPath(string targetPath)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var instrumentationFilePath = Path.Combine(directoryPath, "CoreCover.Instrumentation.dll");
            var targetFilePath = Path.Combine(targetPath, "CoreCover.Instrumentation.dll");

            if (!File.Exists(targetFilePath))
                File.Move(instrumentationFilePath, targetFilePath);
        }

        public void ProcessAssemblies(string[] assemblyPaths)
        {
            var instrumentator = new ReportHandler(new InstrumentatorHandler());
            foreach (var assemblyPath in assemblyPaths)
            {
                var shadowAssemblyPath = Path.ChangeExtension(assemblyPath, "orig.dll");
                RenameOriginalAssembly(assemblyPath, shadowAssemblyPath);
                using (var assembly = LoadAssembly(shadowAssemblyPath))
                {
                    instrumentator.Handle(assembly);
                    assembly.Write(assemblyPath, new WriterParameters { WriteSymbols = true });
                }

                CleanTempFiles(Path.GetDirectoryName(assemblyPath));
            }
        }

        private void CleanTempFiles(string folder)
        {
            foreach (var file in Directory.EnumerateFiles(folder, "*.orig.*"))
            {
                if (Regex.IsMatch(Path.GetExtension(file), "^\\.(pdb|dll)$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled))
                    File.Delete(file);
            }
        }

        private void RenameOriginalAssembly(string assemblyPath, string newAssemblyPath)
        {
            var shadowPdbFilePath = Path.ChangeExtension(newAssemblyPath, "pdb");
            var originalPdbFilePath = Path.ChangeExtension(assemblyPath, "pdb");

            File.Move(assemblyPath, newAssemblyPath);
            File.Copy(originalPdbFilePath, shadowPdbFilePath);
        }

        private AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = true };
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);

            return assembly;
        }
    }
}