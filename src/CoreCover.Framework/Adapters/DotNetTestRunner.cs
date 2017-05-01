// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using System.Linq;
using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Model;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Adapters
{
    public class DotNetTestRunner : ITestsRunner
    {
        private readonly IRpcServer _rpcServer;
        private readonly IProcess _process;

        public DotNetTestRunner(IRpcServer rpcServer, IProcess process)
        {
            _rpcServer = rpcServer;
            _process = process;
        }

        public void Run(CoverageContext coverageContext, string testProjectOutputPath)
        {
            var testProjectPath = GetTestProjectPath(testProjectOutputPath);

            _rpcServer.Start(coverageContext);

            /*
             * It is important that the --no-build flag is used, otherwise dotnet-test will run a new build of the project 
             * which will overwrite the assembly files that were instrumented.
             */
            _process.Execute("dotnet", "test --no-build", testProjectPath);

            _rpcServer.Stop();
        }

        private static string GetTestProjectPath(string testProjectOutputPath)
        {
            var absolutePath = GetAbsolutePath(testProjectOutputPath);
            var absolutePathWithTrailingSlash = GetAbsolutePathWithTrailingSlash(absolutePath);
            var testProjectPath = FindTestProjectPath(absolutePathWithTrailingSlash);

            return testProjectPath;
        }

        private static string FindTestProjectPath(string absolutePathWithTrailingSlash)
        {
            if (Directory.Exists(absolutePathWithTrailingSlash) && 
               !Directory.GetFiles(absolutePathWithTrailingSlash, "*.csproj").Any())
            {
                var parent = Directory.GetParent(absolutePathWithTrailingSlash);
                if (parent != null)
                    return FindTestProjectPath(parent.FullName);
            }
            
            return absolutePathWithTrailingSlash;
        }

        private static string GetAbsolutePathWithTrailingSlash(string absolutePath)
        {
            var directorySeparator = Path.DirectorySeparatorChar.ToString();
            if (!absolutePath.EndsWith(directorySeparator))
                absolutePath += directorySeparator;

            return absolutePath;
        }

        private static string GetAbsolutePath(string testProjectOutputPath)
        {
            var fullPath = testProjectOutputPath;
            if (!Path.IsPathRooted(fullPath))
                fullPath = Path.Combine(Directory.GetCurrentDirectory(), testProjectOutputPath);

            return fullPath;
        }
    }
}