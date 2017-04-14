using CoreCover.Framework;

namespace CoreCover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new ConsoleRunner(new ConsoleWrapper(), new Runner())
                .ProcessCommand(args);
        }
    }
}