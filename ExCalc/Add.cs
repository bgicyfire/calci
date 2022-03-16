using System.Collections.Generic;

namespace ExCalc
{
    public class Add : CalcExp
    {
        private readonly CalcExp _left;
        private readonly CalcExp _right;

        public Add(CalcExp left,CalcExp right)
        {
            _left = left;
            _right = right;
        }
        public override int Calc(Dictionary<string, int> vals)
        {
            var result = this._left.Calc(vals) + this._right.Calc(vals);
            return result;
        }
    }
}