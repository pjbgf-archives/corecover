// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework.Model;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface ICoverageReport
    {
        void Export(CoverageContext coverageSession, string reportPath);
    }
}