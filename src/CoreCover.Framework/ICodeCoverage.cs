namespace CoreCover.Framework
{
    public interface ICodeCoverage
    {
        void Run(string testProjectPath, string reportPath);
    }
}