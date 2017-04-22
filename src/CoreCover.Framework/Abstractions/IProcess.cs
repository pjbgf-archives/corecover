// MIT License
// Copyright (c) 2017 Paulo Gomes

namespace CoreCover.Framework.Abstractions
{
    public interface IProcess
    {
        void Execute(string command, string arguments, string workingDirectory);
    }
}