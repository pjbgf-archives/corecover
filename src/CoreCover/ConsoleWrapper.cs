using System;

namespace CoreCover
{
    public class ConsoleWrapper : IConsole
    {
        public void WriteLine(string value)
        {
            Console.Write(value);
        }
    }
}