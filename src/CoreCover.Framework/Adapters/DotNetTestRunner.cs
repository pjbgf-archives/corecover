// MIT License
// Copyright (c) 2017 Paulo Gomes

using System.Diagnostics;
using System.IO;
using CoreCover.Framework.Abstractions;

namespace CoreCover.Framework.Adapters
{
    public class DotNetTestRunner : ITestsRunner
    {
        public void Run(string testProjectOutputPath)
        {
            var testProjectPath = GetTestProjectPath(testProjectOutputPath);

            /*
             * It is important that the --no-build flag is used, otherwise dotnet-test will run a new build of the project 
             * which will overwrite the assembly files that were instrumented.
             */
            var processStartInfo = new ProcessStartInfo("dotnet", $"test --no-build") { WorkingDirectory = testProjectPath };
            using (var process = Process.Start(processStartInfo))
            {
                process.WaitForExit();
            }
        }

        private static string GetTestProjectPath(string testProjectOutputPath)
        {
            var fullPath = testProjectOutputPath;
            if (!Path.IsPathRooted(testProjectOutputPath))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), testProjectOutputPath);

            var testProjectPath = Directory.GetParent(fullPath).Parent.Parent.Parent.FullName;
            return testProjectPath;
        }
    }
}