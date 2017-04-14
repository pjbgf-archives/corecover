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