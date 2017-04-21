// MIT License
// Copyright (c) 2017 Paulo Gomes

using System;
using CoreCover.Framework.Abstractions;

namespace CoreCover.Framework.Adapters
{
    public class ConsoleAdapter : IConsole
    {
        public void WriteLine(string value)
        {
            Console.Write(value);
        }
    }
}