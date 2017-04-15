using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CoreCover.Instrumentation;
using Mono.Cecil;

namespace CoreCover.Framework
{
    public class Instrumentator : IInstrumentator
    {
        private IAssemblyInstrumentationHandler _assemblyInstrumentationHandler;

        public Instrumentator(IAssemblyInstrumentationHandler assemblyInstrumentationHandler)
        {
            _assemblyInstrumentationHandler = assemblyInstrumentationHandler;
        }

        private const string InstrumentationAssemblyName = "CoreCover.Instrumentation.dll";

        public void Process(string folderPath)
        {
            CopyDependenciesTo(folderPath);
            ProcessAssemblies(new string[] { folderPath });
            ReportTracker.WriteReport();
        }

        private void CopyDependenciesTo(string targetPath)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var instrumentationFilePath = Path.Combine(directoryPath, InstrumentationAssemblyName);
            var targetFilePath = Path.Combine(targetPath, InstrumentationAssemblyName);

            if (!File.Exists(targetFilePath))
                File.Move(instrumentationFilePath, targetFilePath);
        }

        public void ProcessAssemblies(string[] assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                var shadowAssemblyPath = Path.ChangeExtension(assemblyPath, "orig.dll");
                RenameOriginalAssembly(assemblyPath, shadowAssemblyPath);
                using (var assembly = LoadAssembly(shadowAssemblyPath))
                {
                    _assemblyInstrumentationHandler.Handle(assembly);
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