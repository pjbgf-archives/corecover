// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreCover.Framework.Abstractions;
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
                if (type.Name == "__CORECOVER__")
                    continue;

                //HACK: Needs refactoring.
                var documentUrl = type.Methods.FirstOrDefault()?.DebugInformation.SequencePoints.FirstOrDefault()?.Document.Url;
                if (string.IsNullOrEmpty(documentUrl))
                    continue;

                var file = new File { FullPath = documentUrl };
                files.Add(file);
                
                var processType = ProcessType(type, file.UniqueId);
                if (processType != null)
                    types.Add(processType);
            }

            coverageModule.Files = files.ToArray();
            coverageModule.Classes = types.ToArray();

            return coverageModule;
        }

        private Class ProcessType(TypeDefinition type, uint fileId)
        {
            var coverageClass = new Class();
            var methods = new List<Method>(type.Methods.Capacity);

            coverageClass.FullName = type.FullName;

            foreach (var method in type.Methods)
                methods.Add(ProcessMethod(method, fileId));

            coverageClass.Summary = new Summary
            {
                NumMethods = type.Methods.Count
            };

            coverageClass.Methods = methods.ToArray();

            return coverageClass;
        }

        private Method ProcessMethod(MethodDefinition method, uint fileId)
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
                uint ordinal = 0;
                coverageMethod.SequencePoints = method.DebugInformation.SequencePoints.Select(x => new SequencePoint
                {
                    Offset = x.Offset,
                    StartLine = x.StartLine,
                    EndLine = x.EndLine,
                    StartColumn = x.StartColumn,
                    EndColumn = x.EndColumn,
                    Ordinal = ordinal++,
                    FileId = fileId
                }).ToArray();

                coverageMethod.Summary.NumSequencePoints = coverageMethod.SequencePoints.Length;
            }

            return coverageMethod;
        }
    }
}