using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CoreCover.Instrumentation;
using Mono.Cecil;
using Mono.Cecil.Pdb;
using OpenCover.Framework.Model;
using File = System.IO.File;
using Module = OpenCover.Framework.Model.Module;

namespace CoreCover
{
    partial class Program
    {
        static void Main(string[] args)
        {
            //TODO: Try to load target dlls through csproj
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("usage: CoreCover [path]MyTests.dll");
                return;
            }
            
            CopyInstrumentationToTargetPath(Path.GetDirectoryName(args[0]));
            new Program().ProcessAssemblies(args);
            ReportTracker.WriteReport();

            var openCoverReport = new OpenCover.Framework.Model.CoverageSession();
            openCoverReport.Summary = new Summary();
            
            var serializer = new XmlSerializer(typeof(CoverageSession),
                new[] { typeof(Module), typeof(OpenCover.Framework.Model.File), typeof(Class) });

            var reportPath = "C:\\git\\corecover\\src\\CoreCover.Sample.Tests\\bin\\Debug\\netcoreapp1.1\\test.xml";
            ExportReport(reportPath, serializer, openCoverReport);
        }

        private static void ExportReport(string reportPath, XmlSerializer serializer, CoverageSession openCoverReport)
        {
            using (var fs = new FileStream(reportPath, FileMode.Create))
            using (var writer = new StreamWriter(fs, new UTF8Encoding()))
            {
                serializer.Serialize(writer, openCoverReport);
            }
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