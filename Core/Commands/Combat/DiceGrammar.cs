using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using Ancora;

namespace RMUD
{
    public class DiceGrammar : Grammar // Todo: Move to core.
    {
        public DiceGrammar()
        {
            DefaultParserFlags = ParserFlags.IGNORE_LEADING_WHITESPACE | ParserFlags.IGNORE_TRAILING_WHITESPACE;
            
            var digits = "0123456789";
            var d = Keyword("d");
            var number = Token(c => digits.Contains(c)).CreateAst("NUMBERLIT");
            var die = ((d + number).CreateAst("SINGLE_DIE") | (number + d + number).CreateAst("MULTIPLE_DICE"));

            var opTable = new OperatorTable();
                        
            opTable.AddOperator("+", 2);
            opTable.AddOperator("-", 2);

            opTable.AddOperator("*", 3);
            
            var statement = LateBound();
            var expression = LateBound();

            var term = LateBound();
            var op = Operator(opTable).CreateAst("OP");
            expression.SetSubParser(Expression(term, op, opTable));
            term.SetSubParser((die.PassThrough() | number.PassThrough()).CreateAst("TERM"));

            this.Root = expression;
            
            //Console.WriteLine("Result: " + CalculateDieRoll(TestParse("4d6"), new Random()));
            //Console.WriteLine("Result: " + CalculateDieRoll(TestParse("4d6+8"), new Random()));
            //Console.WriteLine("Result: " + CalculateDieRoll(TestParse("4d6+5d10+3-1d6"), new Random()));
            //Console.WriteLine("Result: " + CalculateDieRoll(TestParse("4d6+d10"), new Random()));
        }

        public int CalculateDieRoll(AstNode Node, Random Random)
        {
            if (Node.NodeType == "NUMBERLIT")
                return Int32.Parse(Node.Value.ToString());
            else if (Node.NodeType == "SINGLE_DIE")
                return Random.Next(1, Int32.Parse(Node.Children[0].Value.ToString()) + 1);
            else if (Node.NodeType == "MULTIPLE_DICE")
            {
                var r = 0;
                var dieCount = Int32.Parse(Node.Children[0].Value.ToString());
                var sides = Int32.Parse(Node.Children[1].Value.ToString());
                for (var d = 0; d < dieCount; ++d)
                    r += Random.Next(1, sides + 1);
                return r;
            }
            else if (Node.NodeType == "BINARYOP")
            {
                var a = CalculateDieRoll(Node.Children[0], Random);
                var b = CalculateDieRoll(Node.Children[1], Random);
                if (Node.Value.ToString() == "+")
                    return a + b;
                else if (Node.Value.ToString() == "-")
                    return a - b;
                else if (Node.Value.ToString() == "*")
                    return a * b;
                else
                    return 0;
            }
            else
                return 0;
        }

        public AstNode TestParse(String t)
        {
            Console.WriteLine(t);
            var itr = new Ancora.StringIterator(t); // Todo: Unify iterator types
            var r = Root.Parse(itr);
            if (r.ResultType == Ancora.ResultType.Success && !r.StreamState.AtEnd && r.FailReason == null)
                Console.WriteLine("Did not consume all input.");

            if (r.ResultType == Ancora.ResultType.Success && r.StreamState.AtEnd)
            {
                Console.WriteLine("Parsed.");
                EmitAst(r.Node);
            }
            else
            {
                Console.WriteLine("Failed.");
                if (r.FailReason != null)
                    Console.WriteLine(r.FailReason.Message);
                else
                    Console.WriteLine("No fail reason specified.");
            }

            return r.Node;
        }

        static void EmitAst(Ancora.AstNode Node, int Depth = 0)
        {
            Console.WriteLine(new String(' ', Depth) + Node.NodeType + " : " + (Node.Value == null ? "null" : Node.Value.ToString()));
            foreach (var child in Node.Children) EmitAst(child, Depth + 1);
        }
    }
}
