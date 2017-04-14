using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil.Pdb;

namespace CoreCover
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("usage: dotnet corecover.dll TestProjectOutputPath [path]coverage-report.xml");
                return;
            }

            //TODO: Load target dlls through csproj
            var testProjectOutputPath = args.First();
            var reportPath = args.Last();

            var runner = new Runner();
            runner.Run(testProjectOutputPath, reportPath);
        }
    }
}