// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Linq;
using CoreCover.Framework;
using CoreCover.Framework.Model;
using Mono.Cecil;
using Xunit;

namespace CoreCover.Tests.Framework
{
    public class StaticCodeAnalyserShould
    {
        [Fact]
        public void Add_AssemblyModules_Onto_CoverageContext()
        {
            var analyser = new StaticCodeAnalyser();
            var coverageContext = new CoverageContext();
            var assemblyDefinition = AssemblyDefinition.CreateAssembly(new AssemblyNameDefinition("TestAssembly.dll", new Version(1, 0)), "TestAssembly.dll", ModuleKind.Dll);

            analyser.Handle(coverageContext, assemblyDefinition);

            Assert.Equal("TestAssembly", coverageContext.Modules.First().ModuleName);
        }
    }
}