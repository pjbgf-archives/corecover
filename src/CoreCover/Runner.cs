using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using CoreCover.Instrumentation;
using Mono.Cecil;
using OpenCover.Framework.Model;
using File = System.IO.File;
using Module = OpenCover.Framework.Model.Module;

namespace CoreCover
{
    public class Runner : IRunner
    {
        public void Run(string testProjectPath, string reportPath)
        {
            CopyInstrumentationToTargetPath(Path.GetDirectoryName(testProjectPath));
            ProcessAssemblies(new string[] { testProjectPath });
            ReportTracker.WriteReport();

            var openCoverReport = new OpenCover.Framework.Model.CoverageSession();
            openCoverReport.Summary = new Summary();

            var serializer = new XmlSerializer(typeof(CoverageSession),
                new[] { typeof(Module), typeof(OpenCover.Framework.Model.File), typeof(Class) });
            ExportReport(reportPath, serializer, openCoverReport);
        }

        private void ExportReport(string reportPath, XmlSerializer serializer, CoverageSession openCoverReport)
        {
            using (var fs = new FileStream(reportPath, FileMode.Create))
            using (var writer = new StreamWriter(fs, new UTF8Encoding()))
            {
                serializer.Serialize(writer, openCoverReport);
            }
        }

        private void CopyInstrumentationToTargetPath(string targetPath)
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