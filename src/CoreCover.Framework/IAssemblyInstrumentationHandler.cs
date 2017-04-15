using Mono.Cecil;

namespace CoreCover.Framework
{
    public interface IAssemblyInstrumentationHandler
    {
        void Handle(AssemblyDefinition assemblyDefinition);
    }
}