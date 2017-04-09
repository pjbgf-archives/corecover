using System;
using System.Collections.Generic;
using System.Text;
using Mono.Cecil;

namespace CoreCover
{
    public sealed class ReportHandler : AssemblyHandler
    {
        public ReportHandler(AssemblyHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Reporting Assembly: {assemblyDefinition.FullName}");

            base.Handle(assemblyDefinition);
        }
    }
}
