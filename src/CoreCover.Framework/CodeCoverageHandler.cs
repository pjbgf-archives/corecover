using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using CoreCover.Instrumentation;
using Mono.Cecil;
using OpenCover.Framework.Model;
using File = OpenCover.Framework.Model.File;

namespace CoreCover.Framework
{
    public sealed class CodeCoverageHandler : AssemblyInstrumentationHandler
    {
        public CodeCoverageHandler(AssemblyInstrumentationHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(CoverageSession coverageSession, AssemblyDefinition assemblyDefinition)
        {
            Console.WriteLine($"Reporting Assembly: {assemblyDefinition.FullName}");

            var modules = new List<Module>(coverageSession.Modules.Length + assemblyDefinition.Modules.Count);
            modules.AddRange(coverageSession.Modules);

            foreach (var module in assemblyDefinition.Modules)
            {
                modules.Add(ProcessModule(module));
            }

            coverageSession.Modules = modules.ToArray();

            base.Handle(coverageSession, assemblyDefinition);
        }

        private Module ProcessModule(ModuleDefinition module)
        {
            var coverageModule = new Module();
            var types = new List<Class>(module.Types.Count);

            coverageModule.ModuleHash = module.Mvid.ToString();
            coverageModule.ModulePath = Path.GetFullPath(module.FileName);
            coverageModule.ModuleName = Path.GetFileNameWithoutExtension(module.FileName);
            coverageModule.ModuleTime = System.IO.File.GetLastWriteTimeUtc(module.FileName);

            var files = new List<File>(module.Types.Count);

            foreach (var type in module.Types)
            {
                //HACK: Needs refactoring.
                var documentUrl = type.Methods.FirstOrDefault()?.DebugInformation.SequencePoints.FirstOrDefault()?.Document.Url;
                if (!string.IsNullOrEmpty(documentUrl))
                    files.Add(new File { FullPath = documentUrl });

                var processType = ProcessType(type);
                if (processType != null)
                    types.Add(processType);
            }

            coverageModule.Files = files.ToArray();
            coverageModule.Classes = types.ToArray();

            return coverageModule;
        }

        private Class ProcessType(TypeDefinition type)
        {
            var coverageClass = new Class();
            var methods = new List<Method>(type.Methods.Capacity);

            coverageClass.FullName = type.FullName;

            foreach (var method in type.Methods)
                methods.Add(ProcessMethod(method));

            coverageClass.Summary = new Summary
            {
                NumMethods = type.Methods.Count
            };

            coverageClass.Methods = methods.ToArray();

            return coverageClass;
        }

        private Method ProcessMethod(MethodDefinition method)
        {
            var coverageMethod = new Method();

            coverageMethod.FullName = method.FullName;
            coverageMethod.IsConstructor = method.IsConstructor;
            coverageMethod.IsStatic = method.IsStatic;
            coverageMethod.IsSetter = method.IsSetter;
            coverageMethod.IsGetter = method.IsGetter;
            coverageMethod.MetadataToken = method.MetadataToken.ToInt32();

            coverageMethod.Summary = new Summary();

            if (method.DebugInformation.HasSequencePoints)
            {
                coverageMethod.SequencePoints = method.DebugInformation.SequencePoints.Select(x => new SequencePoint
                {
                    Offset = x.Offset,
                    StartLine = x.StartLine,
                    EndLine = x.EndLine,
                    StartColumn = x.StartColumn,
                    EndColumn = x.EndColumn
                }).ToArray();

                coverageMethod.Summary.NumSequencePoints = coverageMethod.SequencePoints.Length;
            }

            return coverageMethod;
        }
    }
}