using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancora
{
    public enum ResultType
    {
        Success,
        Failure,
        HardError
    }

    public struct ParseResult
    {
        public ResultType ResultType;
        public AstNode Node;
        public StringIterator StreamState;
        public ParserFlags Flags;
        public Failure FailReason;

        public bool CheckFlag(ParserFlags Flag)
        {
            return (Flags & Flag) == Flag;
        }

        public ParseResult ApplyFlags(ParserFlags Flags)
        {
            return new ParseResult
            {
                ResultType = ResultType,
                Node = Node,
                StreamState = StreamState,
                Flags = Flags,
                FailReason = FailReason,
            };
        }
        
    }
}
