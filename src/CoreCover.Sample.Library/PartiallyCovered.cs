using System;

namespace CoreCover.Sample.Library
{
    public class PartiallyCovered
    {
        public bool TestedMethod(bool condition)
        {
            bool returnValue;

            if (condition)
            {
                returnValue = false;
            }
            else
            {
                returnValue = true;
            }

            return returnValue;
        }

        public int NotTestedMethod()
        {
            var a = 1;
            var b = 2;
            return a + b;
        }
    }
}
