using System;
using System.Collections.Generic;

namespace ExCalc
{
    public class AssignExp : CalcExp
    {
        private readonly string _left;
        private readonly CalcExp _right;
        private readonly AssignSign _assignSign;

        public AssignExp(string left, CalcExp right, AssignSign assignSign)
        {
            _left = left;
            _right = right;
            _assignSign = assignSign;
        }

        public override int Calc(Dictionary<string, int> vals)
        {
            int result = this._right.Calc(vals);
            if (this._assignSign == AssignSign.Neutral)
            {
                vals[this._left] = result;
            }
            else
            {
                if (!vals.ContainsKey(this._left))
                    throw new IdentifierNotFoundException(this._left);
                switch (this._assignSign)
                {
                    case AssignSign.Add:
                        vals[this._left] += result;
                        break;
                    case AssignSign.Subtract:
                        vals[this._left] -= result;
                        break;
                    case AssignSign.Multiply:
                        vals[this._left] *= result;
                        break;
                    case AssignSign.Divide:
                        vals[this._left] /= result;
                        break;
                    default:
                        throw new ApplicationException("Not supported action");
                }
            }

            return result;
        }

        public enum AssignSign
        {
            Neutral,
            Add,
            Subtract,
            Multiply,
            Divide,
        }
    }
}