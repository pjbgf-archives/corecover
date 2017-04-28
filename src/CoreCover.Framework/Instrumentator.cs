// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using OpenCover.Framework.Model;
using File = System.IO.File;

namespace CoreCover.Framework
{
    public class Instrumentator : IInstrumentator
    {
        private readonly IAssemblyInstrumentationHandler _assemblyInstrumentationHandler;
        private readonly string[] DependencyAssemblyNames = { "CoreCover.Instrumentation.dll", "Google.Protobuf.dll", "Grpc.Core.dll" };
        private readonly ILogger _logger;

        public Instrumentator(ILogger logger, IAssemblyInstrumentationHandler assemblyInstrumentationHandler)
        {
            _assemblyInstrumentationHandler = assemblyInstrumentationHandler;
            _logger = logger;
        }

        public void Process(CoverageSession coverageSession, string folderPath)
        {
            CopyDependenciesTo(folderPath);
            ProcessAssemblies(coverageSession, Directory.GetFiles(folderPath, "*.dll"));
        }

        private void CopyDependenciesTo(string targetPath)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            foreach (var assemblyName in DependencyAssemblyNames)
                CopyDependencyTo(Path.Combine(directoryPath, assemblyName), targetPath);
        }

        private void CopyDependencyTo(string dependencyFilePath, string targetDirectory)
        {
            _logger.LogInformation($"Dependency Location: {dependencyFilePath}");
            var targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(dependencyFilePath));

            if (!File.Exists(targetFilePath))
                File.Copy(dependencyFilePath, targetFilePath);
        }

        public void ProcessAssemblies(CoverageSession coverageSession, string[] assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                var pdbFile = Path.ChangeExtension(assemblyPath, "pdb");
                if (!File.Exists(pdbFile))
                {
                    _logger.LogInformation($"Skipping {assemblyPath}: missing pdb.");
                    continue;
                }

                if (Regex.IsMatch(assemblyPath, "(CoreCover.Extensions.OpenCoverReport|CoreCover.Instrumentation|Test(s){0,1}).dll$"))
                {
                    _logger.LogInformation($"Skipping {assemblyPath}: test assembly.");
                    continue;
                }

                using (var assembly = LoadAssembly(assemblyPath))
                {
                    _assemblyInstrumentationHandler.Handle(coverageSession, assembly);
                    assembly.Write(new WriterParameters { WriteSymbols = true });
                }
            }
        }

        private AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = true, ReadWrite = true };
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);

            return assembly;
        }
    }
}