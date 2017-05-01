// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
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
            var assemblyPaths = Directory.GetFiles(folderPath, "*.dll");
            ProcessAssemblies(coverageContext, assemblyPaths);
        }

        public void ProcessAssemblies(CoverageContext coverageContext, string[] assemblyPaths)
        {
            foreach (var assemblyPath in assemblyPaths)
            {
                _logger.LogInformation($"Processing {assemblyPath}...");

                string reason;
                if (AssemblyShouldBeSkipped(assemblyPath, out reason))
                {
                    _logger.LogInformation($"Skipping {assemblyPath}: {reason}.");
                    continue;
                }

                using (var assembly = LoadAssembly(assemblyPath))
                {
                    _assemblyInstrumentationHandler.Handle(coverageContext, assembly);
                    assembly.Write(new WriterParameters { WriteSymbols = true });
                }
            }
        }

        private bool AssemblyShouldBeSkipped(string assemblyPath, out string reason)
        {
            if (PdbForAssemblyIsMissing(assemblyPath))
                reason = "pdb is missing";
            else if (IsTestAssembly(assemblyPath))
                reason = "test assembly";
            else
                reason = string.Empty;

            return !string.IsNullOrEmpty(reason);
        }

        private static bool PdbForAssemblyIsMissing(string assemblyPath)
        {
            var pdbFilePath = Path.ChangeExtension(assemblyPath, "pdb");
            return !File.Exists(pdbFilePath);
        }

        private static bool IsTestAssembly(string assemblyPath)
        {
            return Regex.IsMatch(assemblyPath, "(CoreCover.Extensions.OpenCoverReport|CoreCover.Instrumentation|Test(s){0,1})+.dll$");
        }

        private AssemblyDefinition LoadAssembly(string assemblyPath)
        {
            var readerParameters = new ReaderParameters { ReadSymbols = true, ReadWrite = true };
            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath, readerParameters);

            return assembly;
        }
    }
}