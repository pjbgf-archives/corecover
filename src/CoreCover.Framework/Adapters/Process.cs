using System.Diagnostics;
using CoreCover.Framework.Abstractions;
using Microsoft.Extensions.Logging;

namespace CoreCover.Framework.Adapters
{
    public class Process : IProcess
    {
        private readonly ILogger _logger;

        public Process(ILogger logger)
        {
            _logger = logger;
        }

        public void Execute(string command, string arguments, string workingDirectory)
        {
            _logger.LogInformation($"Executing: {workingDirectory} {command} {arguments}");
            var processStartInfo = new ProcessStartInfo(command, arguments) { WorkingDirectory = workingDirectory };
            using (var process = System.Diagnostics.Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
        }
    }
}