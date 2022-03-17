using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using ExCalc;

namespace ExCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Calci parser!");
            Console.WriteLine("Please enter your commands, to finish type CTRL+D and enter:");

            try
            {
                string input = "";
                StringBuilder text = new StringBuilder();

                // to type the EOF character and end the input: use CTRL+D, then press <enter>
                while ((input = Console.ReadLine()) != "\u0004")
                {
                    text.AppendLine(input);
                }
                /* Example:
                 i_9O9=8
                 c=3
                 c+=i_9O9+++5
                 n=(1+c)*2+3

                 Example:
                i = 0
                j = ++i
                x = i++ + 5
                y = 5 + 3 * 10
                i += y
                 */

                AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
                CalciLexer calciLexer = new CalciLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(calciLexer);
                CalciParser calciParser = new CalciParser(commonTokenStream);

                CalciParser.SeqContext chatContext = calciParser.seq();
                BasicCalciVisitor visitor = new BasicCalciVisitor();
                visitor.Visit(chatContext);


                var vals = new Dictionary<string, int>();
                foreach (var line in visitor.Lines)
                {
                    line.Calc(vals);
                }

                Console.WriteLine("(" + string.Join(", ", vals.Select(p => $"{p.Key}={p.Value}")) + ")");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}