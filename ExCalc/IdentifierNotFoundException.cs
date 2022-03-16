using System;

namespace ExCalc
{
    public class IdentifierNotFoundException : ApplicationException
    {
        public IdentifierNotFoundException(string iden) : base(iden)
        {
        }
    }
}