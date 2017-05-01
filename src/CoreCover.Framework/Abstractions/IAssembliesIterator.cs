// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using CoreCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IAssemblyIterator
    {
        void ProcessAssembliesInFolder(CoverageContext coverageContext, string folderPath);
    }
}