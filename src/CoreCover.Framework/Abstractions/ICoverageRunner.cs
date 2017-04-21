// MIT License
// Copyright (c) 2017 Paulo Gomes

namespace CoreCover.Framework.Abstractions
{
    public interface ICoverageRunner
    {
        void Run(string testProjectPath, string reportPath);
    }
}