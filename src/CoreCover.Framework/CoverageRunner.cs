// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.IO;
using CoreCover.Framework.Abstractions;
using OpenCover.Framework.Model;
using File = System.IO.File;

namespace CoreCover.Framework
{
    public class CoverageRunner : ICoverageRunner
    {
        private readonly ITestsRunner _testRunner;
        private readonly IInstrumentator _instrumentator;
        private readonly ICoverageReport _coverageReport;

        public CoverageRunner(IInstrumentator instrumentator, ITestsRunner testRunner, ICoverageReport coverageReport)
        {
            _instrumentator = instrumentator;
            _testRunner = testRunner;
            _coverageReport = coverageReport;
        }

        public void Run(string testProjectOutputPath, string reportPath)
        {
            if (string.IsNullOrEmpty(reportPath))
                reportPath = "coverage.xml";

            var coverageSession = new CoverageSession();


            //If executed against self
            if (testProjectOutputPath.Contains("src\\CoreCover.Tests\\bin"))
            {
                var currentDirectory = Directory.GetParent(testProjectOutputPath);
                var binFolder = currentDirectory.Parent.Parent;
                var selfFolder = Path.Combine(Path.Combine(binFolder.FullName, "self"), currentDirectory.Name);

                if (!Directory.Exists(selfFolder))
                    Directory.CreateDirectory(selfFolder);

                CopyFiles(testProjectOutputPath, selfFolder);

                _instrumentator.Process(coverageSession, selfFolder);
                _testRunner.Run(coverageSession, selfFolder);
                _coverageReport.Export(coverageSession, reportPath);

                //if (Directory.Exists(selfFolder))
                //    Directory.Delete(selfFolder, true);
            }
            else
            {
                _instrumentator.Process(coverageSession, testProjectOutputPath);
                _testRunner.Run(coverageSession, testProjectOutputPath);
                _coverageReport.Export(coverageSession, reportPath);
            }

        }

        private static string CopyFiles(string source, string target)
        {
            var files = Directory.GetFiles(source);

            foreach (var file in files)
                File.Copy(file, Path.Combine(target, Path.GetFileName(file)), true);
            
            return source;
        }
    }
}