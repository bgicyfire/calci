using System.Collections.Generic;

namespace ExCalc
{
    public class NumberExp : CalcExp
    {
        private readonly int _value;

        public NumberExp(int value)
        {
            _value = value;
        }

        public override int Calc(Dictionary<string, int> vals)
        {
            return this._value;
        }
    }
}