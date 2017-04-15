using Mono.Cecil;

namespace CoreCover.Framework
{
    public abstract class AssemblyInstrumentationHandler : IAssemblyInstrumentationHandler
    {
        private readonly AssemblyInstrumentationHandler _sucessorHandler;

        protected AssemblyInstrumentationHandler(AssemblyInstrumentationHandler sucessorHandler)
        {
            _sucessorHandler = sucessorHandler;
        }

        public virtual void Handle(AssemblyDefinition assemblyDefinition)
        {
            _sucessorHandler?.Handle(assemblyDefinition);
        }
    }
}
