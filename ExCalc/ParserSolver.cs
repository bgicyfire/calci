using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ExCalc
{
    public class ParserSolver
    {
        private const string PAT_IDENTIFIER = "(?<iden>[A-z]+[A-z,0-9]*)";

        public Dictionary<string, int> ParseCalc(string str, Dictionary<string, int> aList)
        {
            str = str.Trim();
            var match = Regex.Match(str, @"^" + PAT_IDENTIFIER);
            if (!match.Success)
            {
                throw new ApplicationException();
            }

            var targetIdentifier = match.Value;
            var str2 = str.Substring(targetIdentifier.Length).Trim();
            if (str2.StartsWith("="))
            {
                aList[targetIdentifier] = this.Calc(str2.Substring(1).Trim(), aList);
                return aList;
            }

            if (!aList.ContainsKey(targetIdentifier))
            {
                throw new IdentifierNotFoundException(targetIdentifier);
            }

            if (str2.StartsWith("+="))
            {
                aList[targetIdentifier] += this.Calc(str2.Substring(2).Trim(), aList);
            }
            else if (str2.StartsWith("-="))
            {
                aList[targetIdentifier] -= this.Calc(str2.Substring(2).Trim(), aList);
            }
            else if (str2.StartsWith("*="))
            {
                aList[targetIdentifier] *= this.Calc(str2.Substring(2).Trim(), aList);
            }
            else if (str2.StartsWith("/="))
            {
                aList[targetIdentifier] /= this.Calc(str2.Substring(2).Trim(), aList);
            }
            else
            {
                throw new ApplicationException();
            }

            return aList;
        }

        int Calc(string str, Dictionary<string, int> aList)
        {
            if (str.Contains("="))
                throw new ApplicationException("illegal char =");

            var str1 = Regex.Replace(str, $@"{PAT_IDENTIFIER}\s*\+\+", new MatchEvaluator(m =>
            {
                var iden = m.Groups["iden"].Value;
                if (!aList.ContainsKey(iden)) throw new IdentifierNotFoundException(iden);
                return (aList[iden]++).ToString();
            }));

            var str2 = Regex.Replace(str1, $@"\+\+\s*{PAT_IDENTIFIER}", new MatchEvaluator(m =>
            {
                var iden = m.Groups["iden"].Value;
                if (!aList.ContainsKey(iden)) throw new IdentifierNotFoundException(iden);
                return (++aList[iden]).ToString();
            }));

            var str3 = Regex.Replace(str2, $@"{PAT_IDENTIFIER}\s*\-\-", new MatchEvaluator(m =>
            {
                var iden = m.Groups["iden"].Value;
                if (!aList.ContainsKey(iden)) throw new IdentifierNotFoundException(iden);
                return (aList[iden]--).ToString();
            }));

            var str4 = Regex.Replace(str3, $@"\-\-\s*{PAT_IDENTIFIER}", new MatchEvaluator(m =>
            {
                var iden = m.Groups["iden"].Value;
                if (!aList.ContainsKey(iden)) throw new IdentifierNotFoundException(iden);
                return (--aList[iden]).ToString();
            }));

            var str5 = Regex.Replace(str4, PAT_IDENTIFIER, new MatchEvaluator(m =>
            {
                var iden = m.Groups["iden"].Value;
                if (!aList.ContainsKey(iden)) throw new IdentifierNotFoundException(iden);
                return aList[iden].ToString();
            }));
            return this.ArthmeticCalc(str5);
        }

        int ArthmeticCalc(string str)
        {
            var parenthesesRegex = new Regex(@"\((?<subexp>[^\(,\)]+)\)");
            var str1 = str;
            while (parenthesesRegex.IsMatch(str))
            {
                str1 = parenthesesRegex.Replace(str1, new MatchEvaluator(m =>
                {
                    var subexp = m.Value;
                    var res = this.ArthmeticCalc(subexp);
                    return res.ToString();
                }), 1);
            }

            var muldivRegex = new Regex(@"(?<a>\d+)\s*(?<op>(\*|/))\s*(?<b>\d+)");
            var str2 = str1;
            while (muldivRegex.IsMatch(str2))
            {
                str2 = muldivRegex.Replace(str2, new MatchEvaluator(m =>
                {
                    var a = int.Parse(m.Groups["a"].Value);
                    var b = int.Parse(m.Groups["b"].Value);
                    var op = m.Groups["op"].Value;
                    switch (op)
                    {
                        case "*":
                            return (a * b).ToString();
                        case "/":
                            return (a / b).ToString();
                        default:
                            throw new ApplicationException($"Unexpected operation {op}");
                    }
                }), 1);
            }

            var assSubRegex = new Regex(@"(?<a>\d+)\s*(?<op>(\+|\-))\s*(?<b>\d+)");
            var str3 = str2;
            while (assSubRegex.IsMatch(str3))
            {
                str3 = assSubRegex.Replace(str3, new MatchEvaluator(m =>
                {
                    var a = int.Parse(m.Groups["a"].Value);
                    var b = int.Parse(m.Groups["b"].Value);
                    var op = m.Groups["op"].Value;
                    switch (op)
                    {
                        case "+":
                            return (a + b).ToString();
                        case "-":
                            return (a - b).ToString();
                        default:
                            throw new ApplicationException($"Unexpected operation {op}");
                    }
                }), 1);
            }

            var str4 = str3.Trim();
            if (!Regex.IsMatch(str4, @"^\d+$"))
            {
                throw new ApplicationException($"Unexpected calc result '{str4}'");
            }

            return int.Parse(str4);
        }

        string RegexReplacePP(Match match)
        {
            return "";
        }
    }
}