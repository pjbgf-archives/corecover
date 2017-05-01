using CoreCover.Framework.Model;

namespace CoreCover.Framework.Abstractions
{
    public interface IRpcServer
    {
        void Start(CoverageContext coverageSession);
        void Stop();
    }
}