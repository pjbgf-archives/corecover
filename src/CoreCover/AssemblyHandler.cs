using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace CoreCover
{
    public abstract class AssemblyHandler
    {
        private readonly AssemblyHandler _sucessorHandler;

        protected AssemblyHandler(AssemblyHandler sucessorHandler)
        {
            _sucessorHandler = sucessorHandler;
        }

        public virtual void Handle(AssemblyDefinition assemblyDefinition)
        {
            _sucessorHandler?.Handle(assemblyDefinition);
        }
    }
}
