using System.Collections.Generic;

namespace ExCalc
{
    public class VariableExp : CalcExp
    {
        private readonly string _name;

        public VariableExp(string name)
        {
            _name = name;
        }

        public override int Calc(Dictionary<string, int> vals)
        {
            return vals[this._name];
        }
    }
}