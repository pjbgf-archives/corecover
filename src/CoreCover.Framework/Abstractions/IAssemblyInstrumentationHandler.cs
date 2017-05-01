// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework.Model;
using Mono.Cecil;

namespace CoreCover.Framework.Abstractions
{
    public interface IAssemblyInstrumentationHandler
    {
        void Handle(CoverageContext coverageContext, AssemblyDefinition assemblyDefinition);
    }
}