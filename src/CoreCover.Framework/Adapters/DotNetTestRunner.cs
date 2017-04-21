// MIT License
// Copyright (c) 2017 Paulo Gomes

using System;
using System.Diagnostics;

namespace CoreCover.Framework.Adapters
{
    public class DotNetTestRunner : ITestsRunner
    {
        public void Run(string testProjectPath)
        {
            var processStartInfo = new ProcessStartInfo("dotnet", $"test --no-build");
            processStartInfo.WorkingDirectory = testProjectPath;
            
            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
        }
    }
}