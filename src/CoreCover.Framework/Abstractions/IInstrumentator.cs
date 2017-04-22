// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IInstrumentator
    {
        void Process(CoverageSession coverageSession, string folderPath);
    }
}