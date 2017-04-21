// MIT License
// Copyright (c) 2017 Paulo Gomes

using System;

namespace CoreCover.Framework
{
    public class ConsoleWrapper : IConsole
    {
        public void WriteLine(string value)
        {
            Console.Write(value);
        }
    }
}