using Mono.Cecil;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public interface IAssemblyInstrumentationHandler
    {
        void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition);
    }
}