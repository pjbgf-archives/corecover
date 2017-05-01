using System.IO;
using System.Reflection;
using CoreCover.Framework.Abstractions;

namespace CoreCover.Framework
{
    public class CoverageDependencies : ICoverageDependencies
    {
        private static readonly string[] DependencyAssemblyNames = { "CoreCover.Instrumentation.dll", "Google.Protobuf.dll", "Grpc.Core.dll" };
        
        public void DeployTo(string targetPath)
        {
            var directoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            foreach (var assemblyName in DependencyAssemblyNames)
                CopyDependency(Path.Combine(directoryPath, assemblyName), targetPath);
        }

        private static void CopyDependency(string dependencyFilePath, string targetDirectory)
        {
            var targetFilePath = Path.Combine(targetDirectory, Path.GetFileName(dependencyFilePath));
            if (!File.Exists(targetFilePath))
                File.Copy(dependencyFilePath, targetFilePath);
        }
    }
}