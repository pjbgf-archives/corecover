// MIT License
// Copyright (c) 2017 Paulo Gomes

namespace CoreCover.Framework
{
    public interface ICodeCoverage
    {
        void Run(string testProjectPath, string reportPath);
    }
}