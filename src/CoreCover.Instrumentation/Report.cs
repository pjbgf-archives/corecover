using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreCover.Instrumentation
{
    public class Report
    {
        public Report()
        {
            ReportFiles = new List<ReportFile>();
        }

        public List<ReportFile> ReportFiles { get; set; }

        public void AddLine(string fileName, int lineNumber)
        {
            if (ReportFiles.All(x => x.DocumentUrl != fileName))
                ReportFiles.Add(new ReportFile(fileName));

            ReportFiles.First(x => x.DocumentUrl == fileName).FileLines.Add(new FileLine { LineNumber = lineNumber });
        }

        public void MarkLineAsCovered(string fileName, int lineNumber)
        {
            ReportFiles.First(x => x.DocumentUrl == fileName)
                .FileLines.First(x => x.LineNumber == lineNumber)
                .IsCovered = true;
        }
    }

    public class ReportFile
    {
        public ReportFile(string documentUrl)
        {
            FileLines = new List<FileLine>();
            DocumentUrl = documentUrl;
        }

        public string DocumentUrl { get; set; }
        public List<FileLine> FileLines { get; set; }
    }

    public class FileLine
    {
        public int LineNumber { get; set; }
        public bool IsCovered { get; set; }
    }
}