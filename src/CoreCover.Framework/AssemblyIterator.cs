// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Model;
using Microsoft.Extensions.Logging;
using Mono.Cecil;
using File = System.IO.File;

namespace CoreCover.Framework
{
    public class AssemblyIterator : IAssemblyIterator
    {
        private readonly IAssemblyHandler _assemblyInstrumentationHandler;
        private readonly ILogger _logger;

        public AssemblyIterator(ILogger logger, IAssemblyHandler assemblyInstrumentationHandler)
        {
            _assemblyInstrumentationHandler = assemblyInstrumentationHandler;
            _logger = logger;
        }

        public void ProcessAssembliesInFolder(CoverageContext coverageContext, string folderPath)
        {
            ProcessAssemblies(coverageContext, Directory.GetFiles(folderPath, "*.dll"));
        }

        public void ProcessAssemblies(CoverageContext coverageContext, string[] assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                _logger.LogInformation($"Processing {assemblyPath}...");
                var pdbFile = Path.ChangeExtension(assemblyPath, "pdb");
                if (!File.Exists(pdbFile))
                {
                    _logger.LogInformation($"Skipping {assemblyPath}: missing pdb.");
                    continue;
                }

                if (Regex.IsMatch(assemblyPath, "(CoreCover.Extensions.OpenCoverReport|CoreCover.Instrumentation|Test(s){0,1})+.dll$"))
                {
                    _logger.LogInformation($"Skipping {assemblyPath}: test assembly.");
                    continue;
                }

                using (var assembly = LoadAssembly(assemblyPath))
                {
                    _assemblyInstrumentationHandler.Handle(coverageContext, assembly);
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