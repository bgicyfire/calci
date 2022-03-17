using System.Collections.Generic;

namespace ExCalc
{
    public class PostfixDecrement : CalcExp
    {
        private readonly string _identifier;

        public PostfixDecrement(string identifier)
        {
            _identifier = identifier;
        }
        public override int Calc(Dictionary<string, int> vals)
        {
            if (!vals.ContainsKey(this._identifier))
                throw new IdentifierNotFoundException(this._identifier);
            var result = vals[this._identifier]--;
            return result;
        }
    }
}