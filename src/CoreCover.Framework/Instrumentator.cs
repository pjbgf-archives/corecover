using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CoreCover.Instrumentation;
using Mono.Cecil;

namespace CoreCover.Framework
{
    public class Instrumentator : IInstrumentator
    {
        private readonly IAssemblyInstrumentationHandler _assemblyInstrumentationHandler;

        public Instrumentator(IAssemblyInstrumentationHandler assemblyInstrumentationHandler)
        {
            _assemblyInstrumentationHandler = assemblyInstrumentationHandler;
        }

        private const string InstrumentationAssemblyName = "CoreCover.Instrumentation.dll";

        public void Process(string folderPath)
        {
            CopyDependenciesTo(folderPath);
            ProcessAssemblies(Directory.GetFiles(folderPath, "*.dll"));
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
                var pdbFile = Path.ChangeExtension(assemblyPath, "pdb");
                if (!File.Exists(pdbFile))
                    continue;
                
                using (var assembly = LoadAssembly(assemblyPath))
                {
                    _assemblyInstrumentationHandler.Handle(assembly);
                    assembly.Write(new WriterParameters { WriteSymbols = true });
                }
            }
        }

        private AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = true, ReadWrite = true};
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);

            return assembly;
        }
    }
}