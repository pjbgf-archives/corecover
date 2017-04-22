// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using Mono.Cecil;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public abstract class AssemblyInstrumentationHandler : IAssemblyInstrumentationHandler
    {
        private readonly AssemblyInstrumentationHandler _sucessorHandler;

        protected AssemblyInstrumentationHandler(AssemblyInstrumentationHandler sucessorHandler)
        {
            _sucessorHandler = sucessorHandler;
        }

        public virtual void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition)
        {
            _sucessorHandler?.Handle(coverageSession, assemblyDefinition);
        }
    }
}
