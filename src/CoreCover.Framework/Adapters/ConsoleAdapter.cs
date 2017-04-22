// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

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