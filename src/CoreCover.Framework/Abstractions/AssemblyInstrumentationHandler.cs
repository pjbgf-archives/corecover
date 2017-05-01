// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework.Model;
using Mono.Cecil;

namespace CoreCover.Framework.Abstractions
{
    public abstract class AssemblyInstrumentationHandler : IAssemblyInstrumentationHandler
    {
        private readonly AssemblyInstrumentationHandler _sucessorHandler;

        protected AssemblyInstrumentationHandler(AssemblyInstrumentationHandler sucessorHandler)
        {
            _sucessorHandler = sucessorHandler;
        }

        public virtual void Handle(CoverageContext coverageContext, AssemblyDefinition assemblyDefinition)
        {
            _sucessorHandler?.Handle(coverageContext, assemblyDefinition);
        }
    }
}
