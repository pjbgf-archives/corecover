using System.Diagnostics;
using CoreCover.Framework.Abstractions;

namespace CoreCover.Framework.Adapters
{
    public class Process : IProcess
    {
        public void Execute(string command, string arguments, string workingDirectory)
        {
            var processStartInfo = new ProcessStartInfo(command, arguments) { WorkingDirectory = workingDirectory };
            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
        }
    }
}