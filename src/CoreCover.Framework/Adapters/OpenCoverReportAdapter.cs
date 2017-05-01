// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CoreCover.Framework.Abstractions;
using CoreCover.Framework.Model;
using OpenCover.Framework.Model;
using BranchPoint = OpenCover.Framework.Model.BranchPoint;
using SequencePoint = OpenCover.Framework.Model.SequencePoint;

namespace CoreCover.Framework.Adapters
{
    public class OpenCoverReportAdapter : ICoverageReport
    {
        public void Export(CoverageContext coverageContext, string reportPath)
        {
            var coverageSession = ConvertTo(coverageContext);
            new CoverageStats().Consolidate(coverageSession);
            GenerateReport(coverageSession, reportPath);
        }

        private static CoverageSession ConvertTo(CoverageContext coverageContext)
        {
            var coverageSession = new CoverageSession();
            coverageSession.Modules = coverageContext.Modules.Select(x => new OpenCover.Framework.Model.Module
                {
                    ModuleHash = x.ModuleHash,
                    ModuleName = x.ModuleName,
                    ModulePath = x.ModulePath,
                    ModuleTime = x.ModuleTime,
                    Files = x.FileReferences.Select(f => new OpenCover.Framework.Model.File {
                        FullPath = f.FilePath,
                        UniqueId = Convert.ToUInt32(f.UniqueId)
                    }).ToArray(),
                    Classes = x.Types.Select(t => new Class
                    {
                        FullName = t.FullName,
                        Methods = t.Methods.Select(m => new OpenCover.Framework.Model.Method
                        {
                            FullName = m.FullName,
                            IsGetter = m.IsGetter,
                            IsConstructor = m.IsConstructor,
                            IsSetter = m.IsSetter,
                            IsStatic = m.IsStatic,
                            Visited = m.Executed,
                            SequencePoints = m.SequencePoints == null ? new SequencePoint[] { } : m.SequencePoints.Select(s => new OpenCover.Framework.Model.SequencePoint
                            {
                                EndLine = s.EndLine,
                                Offset = s.Offset,
                                Ordinal = s.Ordinal,
                                StartLine = s.StartLine,
                                StartColumn = s.StartColumn,
                                EndColumn = s.EndColumn,
                                VisitCount = s.ExecutionCount,
                                FileId = Convert.ToUInt32(s.FileReference.UniqueId)
                            }).ToArray(),
                            BranchPoints = m.BranchPoints == null ? new BranchPoint []{} : m.BranchPoints.Select(b => new OpenCover.Framework.Model.BranchPoint
                            {
                                Ordinal = b.Ordinal,
                                Offset = b.Offset,
                                EndOffset = b.EndOffset,
                                OffsetPoints = b.OffsetPoints,
                                FileId = Convert.ToUInt32(b.FileReference.UniqueId)
                            }).ToArray()
                        }).ToArray()
                    }).ToArray()
                })
                .ToArray();
            return coverageSession;
        }

        private void GenerateReport(CoverageSession coverageSession, string reportPath)
        {
            var serializer = new XmlSerializer(typeof(CoverageSession),
                new[] { typeof(OpenCover.Framework.Model.Module), typeof(OpenCover.Framework.Model.File), typeof(Class) });
            ExportReport(reportPath, serializer, coverageSession);
        }

        private void ExportReport(string reportPath, XmlSerializer serializer, CoverageSession openCoverReport)
        {
            using (var stream = new FileStream(reportPath, FileMode.Create))
            using (var writer = new StreamWriter(stream, new UTF8Encoding()))
            {
                serializer.Serialize(writer, openCoverReport);
            }
        }
    }
}