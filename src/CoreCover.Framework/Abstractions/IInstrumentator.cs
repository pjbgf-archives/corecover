// MIT License
// Copyright (c) 2017 Paulo Gomes

using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IInstrumentator
    {
        void Process(CoverageSession coverageSession, string folderPath);
    }
}