using System;
using OpenCover.Framework.Model;

namespace CoreCover.Framework
{
    public interface IRpcServer
    {
        void Start(CoverageSession coverageSession);
        void Stop();
    }
}