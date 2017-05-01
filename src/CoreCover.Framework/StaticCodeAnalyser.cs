// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.IO;
using System.Linq;
using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Model;
using Mono.Cecil;
using Type = CoreCover.Framework.Model.Type;

namespace CoreCover.Framework
{
    public sealed class StaticCodeAnalyser : AssemblyHandler
    {
        public StaticCodeAnalyser() : this(null)
        {
        }

        public StaticCodeAnalyser(AssemblyHandler sucessorHandler) : base(sucessorHandler)
        {
        }

        public override void Handle(CoverageContext coverageContext, AssemblyDefinition assemblyDefinition)
        {
            foreach (var module in assemblyDefinition.Modules)
            {
                coverageContext.AddModule(ProcessModule(module));
            }

            base.Handle(coverageContext, assemblyDefinition);
        }

        private Module ProcessModule(ModuleDefinition module)
        {
            var coverageModule = new Module();

            coverageModule.ModuleHash = module.Mvid.ToString();
            coverageModule.ModuleName = Path.GetFileNameWithoutExtension(module.Name);

            if (!string.IsNullOrEmpty(module.FileName))
            {
                coverageModule.ModulePath = Path.GetFullPath(module.FileName);
                coverageModule.ModuleTime = System.IO.File.GetLastWriteTimeUtc(module.FileName);
            }

            foreach (var type in module.Types)
            {
                if (string.Compare(type.Name, "__CORECOVER__", StringComparison.CurrentCultureIgnoreCase) == 0 || type.IsInterface)
                    continue;

                //HACK: Needs refactoring.
                var documentUrl = type.Methods.FirstOrDefault()?.DebugInformation.SequencePoints.FirstOrDefault()?.Document.Url;
                if (string.IsNullOrEmpty(documentUrl))
                    continue;

                var fileReference = coverageModule.AddFileReference(documentUrl);
                var processType = ProcessType(type, fileReference);
                if (processType != null)
                    coverageModule.AddType(processType);
            }

            return coverageModule;
        }

        private Type ProcessType(TypeDefinition type, FileReference fileReference)
        {
            var coverageClass = new Type(type.FullName, type.Methods.Count);

            foreach (var method in type.Methods)
                coverageClass.AddMethod(ProcessMethod(method, fileReference));

            return coverageClass;
        }

        private Method ProcessMethod(MethodDefinition method, FileReference fileReference)
        {
            var coverageMethod = new Method();

            coverageMethod.FullName = method.FullName;
            coverageMethod.IsConstructor = method.IsConstructor;
            coverageMethod.IsStatic = method.IsStatic;
            coverageMethod.IsSetter = method.IsSetter;
            coverageMethod.IsGetter = method.IsGetter;
            coverageMethod.MetadataToken = method.MetadataToken.ToInt32();

            if (method.DebugInformation.HasSequencePoints)
            {
                coverageMethod.AddSequencePoints(GetSequencePoints(method, fileReference));
                coverageMethod.AddBranchPoints(GetBranchPoints(method, fileReference));
            }

            return coverageMethod;
        }

        private static SequencePoint[] GetSequencePoints(MethodDefinition method, FileReference fileReference)
        {
            uint ordinal = 0;
            return method.DebugInformation.SequencePoints.Select(x => new SequencePoint
            {
                Offset = x.Offset,
                StartLine = x.StartLine,
                EndLine = x.EndLine,
                StartColumn = x.StartColumn,
                EndColumn = x.EndColumn,
                Ordinal = ordinal++,
                FileReference = fileReference
            }).ToArray();
        }

        private static BranchPoint[] GetBranchPoints(MethodDefinition method, FileReference fileReference)
        {
            uint ordinal = 0;
            return method.DebugInformation.GetScopes()
                .Select(x =>
                {
                    var branchPoint = new BranchPoint
                    {
                        Offset = x.Start.Offset,
                        EndOffset = x.End.Offset,
                        Ordinal = ordinal++,
                        FileReference = fileReference
                    };

                    if (x.HasScopes)
                        using (var enumerator = x.Scopes.GetEnumerator())
                            while (enumerator.MoveNext())
                            {
                                branchPoint.OffsetPoints.Add(enumerator.Current.Start.Offset);
                            }

                    return branchPoint;
                }).ToArray();
        }
    }
}