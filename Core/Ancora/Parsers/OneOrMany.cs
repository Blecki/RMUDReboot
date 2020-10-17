using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancora.Parsers
{
    public class OneOrMany : Parser
    {
        private Parser SubParser;

        public OneOrMany(Parser SubParser)
        {
            this.SubParser = SubParser;
        }

        protected override Parser ImplementClone()
        {
            return new OneOrMany(SubParser);
        }

        protected override ParseResult ImplementParse(StringIterator InputStream)
        {
            var r = new AstNode { NodeType = String.IsNullOrEmpty(AstNodeType) ? "UNNAMED" : AstNodeType };
            AstNode passThroughChild = null;

            while (true)
            {
                var subResult = SubParser.Parse(InputStream);
                if (subResult.ResultType == ResultType.Success)
                {
                    InputStream = subResult.StreamState;
                    if (subResult.Node != null)
                    {
                        if (subResult.CheckFlag(ParserFlags.PASSTHROUGH) && passThroughChild == null)
                            passThroughChild = subResult.Node;
                        else if (subResult.CheckFlag(ParserFlags.FLATTEN))
                            r.Children.AddRange(subResult.Node.Children);
                        else if (subResult.CheckFlag(ParserFlags.VALUE))
                            r.Value = subResult.Node.Value;
                        else
                            r.Children.Add(subResult.Node);
                    }
                }
                else if (subResult.ResultType == ResultType.HardError) return subResult;
                else return new ParseResult
                    {
                        ResultType = r.Children.Count > 0 ? ResultType.Success : ResultType.Failure, //Must match at least 1
                        Node = passThroughChild == null ? r : passThroughChild,
                        StreamState = InputStream,
                        Flags = Flags
                    };
            }
        }
    }
}
