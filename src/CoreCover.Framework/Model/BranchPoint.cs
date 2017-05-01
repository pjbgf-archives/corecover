// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.Collections.Generic;

namespace CoreCover.Framework.Model
{
    public class BranchPoint
    {
        public BranchPoint()
        {
            OffsetPoints = new List<int>();
        }

        public int Offset { get; set; }
        public int EndOffset { get; set; }
        public uint Ordinal { get; set; }
        public FileReference FileReference { get; set; }
        public List<int> OffsetPoints { get; }
    }
}