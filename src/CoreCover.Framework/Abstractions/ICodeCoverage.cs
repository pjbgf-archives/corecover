// MIT License
// Copyright (c) 2017 Paulo Gomes

namespace CoreCover.Framework.Abstractions
{
    public interface ICodeCoverage
    {
        void Run(string testProjectPath, string reportPath);
    }
}