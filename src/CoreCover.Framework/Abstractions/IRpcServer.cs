using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IRpcServer
    {
        void Start(CoverageSession coverageSession);
        void Stop();
    }
}