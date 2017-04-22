// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

namespace CoreCover.Sample.Library
{
    public class FullyCovered
    {
        public int TestedMethod(int input)
        {
            var a = input;
            var b = 2;

            if (a > 0)
                a *= 2;

            return a + b;
        }
    }
}
