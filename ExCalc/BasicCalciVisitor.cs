using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using Antlr4.Runtime.Tree;

namespace ExCalc
{
    public class BasicCalciVisitor : CalciBaseVisitor<CalcExp>
    {
        public List<AssignExp> Lines = new List<AssignExp>();

        public override CalcExp VisitLine(CalciParser.LineContext context)
        {
            CalciParser.VarbContext variable = context.varb();
            var right = Visit(context.add_sub());
            var assigner = context.ASSIGN_OP();
            AssignExp line = new AssignExp(variable.GetText(), right, AssignExp.ParseSign(assigner.GetText()));
            Lines.Add(line);

            return line;
        }

        public override CalcExp VisitAdd_sub(CalciParser.Add_subContext context)
        {
            if (context.add_sub() == null)
            {
                return Visit(context.mul_div());
            }

            var left = Visit(context.add_sub());
            var right = Visit(context.mul_div());
            if (context.ADD_SUB_OP().GetText() == "+")
            {
                return new Add(left, right);
            }

            if (context.ADD_SUB_OP().GetText() == "-")
            {
                return new Subtract(left, right);
            }

            throw new ApplicationException("Unexpected add_sub operator");
        }

        public override CalcExp VisitMul_div(CalciParser.Mul_divContext context)
        {
            if (context.mul_div() == null)
            {
                return Visit(context.atom());
            }

            if (context.MUL_DIV_OP().GetText() == "*")
            {
                var left = Visit(context.mul_div());
                var right = Visit(context.atom());
                return new Multiply(left, right);
            }

            if (context.MUL_DIV_OP().GetText() == "/")
            {
                var left = Visit(context.mul_div());
                var right = Visit(context.atom());
                return new Divide(left, right);
            }
            throw new ApplicationException("Unexpected mul_div operator");
        }

        public override CalcExp VisitAtom(CalciParser.AtomContext context)
        {
            if (context.NUMBB() != null)
            {
                return  new NumberExp(int.Parse(context.NUMBB().GetText()));
                //return Visit(context.NUMBB());
            }

            if (context.VARB() != null)
            {
                return  new VariableExp( context.VARB().GetText());
                //return Visit(context.VARB());
            }

            if (context.add_sub() != null)
            {
                 return Visit(context.add_sub());
            }
            throw new ApplicationException("Unexpected");
        }

        public override CalcExp VisitNum(CalciParser.NumContext context)
        {
            return base.VisitNum(context);
        }
    }
}