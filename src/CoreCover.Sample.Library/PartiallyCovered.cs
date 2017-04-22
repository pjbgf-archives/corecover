// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

namespace CoreCover.Sample.Library
{
    public class PartiallyCovered
    {
        public bool TestedMethod(bool condition)
        {
            bool returnValue;

            if (condition)
            {
                returnValue = false; // Should not be covered
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
