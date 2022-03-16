using System.Collections.Generic;

namespace ExCalc
{
    public abstract class CalcExp
    {
        public abstract int Calc(Dictionary<string, int> vals);
    }
}