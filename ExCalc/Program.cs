using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var parserSolver = new ParserSolver();
            var aList = new Dictionary<string, int>();
            parserSolver.ParseCalc("i = 0", aList);
            parserSolver.ParseCalc("j = ++i", aList);
            parserSolver.ParseCalc("x = i++ + 5", aList);
            parserSolver.ParseCalc("y = 5 + 3 * 10", aList);
            parserSolver.ParseCalc("i += y", aList);

            /*parserSolver.ParseCalc("i=9", aList);
            parserSolver.ParseCalc("y=i++", aList);
            parserSolver.ParseCalc("x =+++i", aList);*/
            var matches = Regex.Matches("aRRRRb", "(a.*)(.*b)");
        }
    }

    public class Parser
    {
        private const string PAT_IDENTIFIER = "(?<iden>[A-z]+[A-z,0-9]*)";

        CalcExp Parse(string str)
        {
            var stack = new Stack<ParseTerm>();
            if (!this.P(str, stack))
            {
                throw new ApplicationException("Unparsable string");
            }

            return this.PtoCalcExp(stack.ToArray());
        }

        private bool P(string str, Stack<ParseTerm> stack)
        {
            str = str.Trim();
            if (str.StartsWith("("))
            {
                stack.Push(new ParseTerm("(", ParseTerm.TermType.OpenPar));
                return this.P(str.Substring(1), stack);
            }

            if (str.StartsWith(")"))
            {
                stack.Push(new ParseTerm(")", ParseTerm.TermType.ClosePar));
                return this.P(str.Substring(1), stack);
            }

            if (str.StartsWith("++"))
            {
                stack.Push(new ParseTerm("++", ParseTerm.TermType.PlusPlus));
                return this.P(str.Substring(2), stack);
            }

            if (str.StartsWith("+"))
            {
                stack.Push(new ParseTerm("+", ParseTerm.TermType.Plus));
                return this.P(str.Substring(1), stack);
            }

            if (str.StartsWith("--"))
            {
                stack.Push(new ParseTerm("++", ParseTerm.TermType.MinusMinus));
                return this.P(str.Substring(2), stack);
            }

            if (str.StartsWith("-"))
            {
                stack.Push(new ParseTerm("-", ParseTerm.TermType.Minus));
                return this.P(str.Substring(1), stack);
            }

            if (str.StartsWith("*"))
            {
                stack.Push(new ParseTerm("*", ParseTerm.TermType.Multiply));
                return this.P(str.Substring(1), stack);
            }

            if (str.StartsWith("/"))
            {
                stack.Push(new ParseTerm("/", ParseTerm.TermType.Divide));
                return this.P(str.Substring(1), stack);
            }

            Match match;
            match = Regex.Match(str, PAT_IDENTIFIER);
            if (match.Success)
            {
                var val = match.Groups["iden"].Value;
                stack.Push(new ParseTerm(val, ParseTerm.TermType.Identifier));
                return this.P(str.Substring(val.Length), stack);
            }

            match = Regex.Match(str, @"^\d+");
            if (match.Success)
            {
                var val = match.Value;
                stack.Push(new ParseTerm(val, ParseTerm.TermType.Num));
                return this.P(str.Substring(val.Length), stack);
            }

            match = Regex.Match(str, @"^(\+|\-|\*|/){0,1}=");
            if (match.Success)
            {
                var val = match.Value;
                stack.Push(new ParseTerm(val, ParseTerm.TermType.Assign));
                return this.P(str.Substring(val.Length), stack);
            }

            return true;
        }

        private CalcExp PtoCalcExp(ParseTerm[] arr)
        {
            if (arr.Last().TType == ParseTerm.TermType.ClosePar)
            {
                for (int i = arr.Length - 2; i > 0; i--)
                {
                    if (arr[i].TType == ParseTerm.TermType.OpenPar)
                    {
                    }
                }
            }

            throw new NotImplementedException();
        }
    }

    public class ParseTerm
    {
        public TermType TType { get; set; }
        public string Value { get; set; }

        public ParseTerm(string value, TermType ttype)
        {
            this.Value = value;
            this.TType = ttype;
        }

        public enum TermType
        {
            Num,
            PlusPlus,
            MinusMinus,
            Plus,
            Minus,
            Multiply,
            Divide,
            Identifier,
            OpenPar,
            ClosePar,
            Assign,
        }
    }
}