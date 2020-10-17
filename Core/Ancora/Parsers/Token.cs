using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancora.Parsers
{
    public class Token : Parser
    {
        private Func<char, bool> IsLegalCharacter;

        public Token(Func<char,bool> IsLegalCharacter)
        {
            this.IsLegalCharacter = IsLegalCharacter;
        }

        protected override Parser ImplementClone()
        {
            return new Token(IsLegalCharacter);
        }

        protected override ParseResult ImplementParse(StringIterator InputStream)
        {
            var text = "";
            while (!InputStream.AtEnd && IsLegalCharacter(InputStream.Next))
            {
                text += InputStream.Next;
                InputStream = InputStream.Advance();
            }

            return new ParseResult
            {
                ResultType = String.IsNullOrEmpty(text) ? ResultType.Failure : ResultType.Success,
                Node = new AstNode
                {
                    NodeType = String.IsNullOrEmpty(AstNodeType) ? "UNNAMED" : AstNodeType,
                    Value = text
                },
                StreamState = InputStream,
                Flags = Flags
            };
        }
    }
}
