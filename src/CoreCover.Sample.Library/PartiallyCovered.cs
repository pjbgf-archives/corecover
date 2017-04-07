using System;

namespace CoreCover.Sample.Library
{
    public class PartiallyCovered
    {
        public bool TestedMethod()
        {
            return true;
        }

        public void NotTestedMethod()
        {
            var a = 1;
            var b = 2;
            var c = a + b;
        }
    }
}
