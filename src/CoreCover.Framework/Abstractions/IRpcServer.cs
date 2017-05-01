using CoreCover.Framework.Model;
using OpenCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IRpcServer
    {
        void Start(CoverageContext coverageSession);
        void Stop();
    }
}