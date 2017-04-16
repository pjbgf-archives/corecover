using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public interface IInstrumentator
    {
        void Process(CoverageSession coverageSession, string folderPath);
    }
}