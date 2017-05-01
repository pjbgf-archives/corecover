// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

namespace CoreCover.Framework.Model
{
    public class SequencePoint
    {
        public int Offset { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public uint Ordinal { get; set; }
        public FileReference FileReference { get; set; }
        public int ExecutionCount { get; set; }
    }
}