using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ancora
{
    public class Grammar
    {
        public Parser Root = null;
        public ParserFlags DefaultParserFlags = ParserFlags.NONE;

        #region Parser Factory Functions

        public Parsers.Sequence Sequence(params Parser[] SubParsers)
        {
            var r = new Parsers.Sequence(SubParsers);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Alternative Alternative(params Parser[] SubParsers)
        {
            var r = new Parsers.Alternative(SubParsers);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Character Character(Char C)
        {
            var r = new Parsers.Character(C);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Expression Expression(Parser TermParser, Parser OperatorParser, OperatorTable OperatorTable)
        {
            var r = new Parsers.Expression(TermParser, OperatorParser, OperatorTable);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Identifier Identifier(Func<Char, bool> IsLegalStartCharacter, Func<Char, bool> IsLegalCharacter)
        {
            var r = new Parsers.Identifier(IsLegalStartCharacter, IsLegalCharacter);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.KeyWord Keyword(String Word)
        {
            var r = new Parsers.KeyWord(Word);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.LateBound LateBound()
        {
            var r = new Parsers.LateBound();
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Maybe Maybe(Parser SubParser)
        {
            var r = new Parsers.Maybe(SubParser);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.NoneOrMany NoneOrMany(Parser SubParser)
        {
            var r = new Parsers.NoneOrMany(SubParser);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.OneOrMany OneOrMany(Parser SubParser)
        {
            var r = new Parsers.OneOrMany(SubParser);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Operator Operator(OperatorTable OperatorTable)
        {
            var r = new Parsers.Operator(OperatorTable);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Token Token(Func<Char, bool> IsLegalCharacter)
        {
            var r = new Parsers.Token(IsLegalCharacter);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Debug Debug(Action<StringIterator> CallOnParse)
        {
            var r = new Parsers.Debug(CallOnParse);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.Debug Debug(String Message)
        {
            var r = new Parsers.Debug(p => Console.WriteLine(Message));
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parsers.HardError HardError(Parser SubParser)
        {
            var r = new Parsers.HardError(SubParser);
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parser DelimitedList(Parser TermParser, Parser SeperatorParser)
        {
            var r = Sequence(
                    TermParser,
                    NoneOrMany(
                        Sequence(
                            SeperatorParser,
                            TermParser.PassThrough()
                        )
                    ).Flatten()
                );
            r.Flags = DefaultParserFlags;
            return r;
        }

        public Parser DelimitedList(Parser TermParser, char SeperatorChar)
        {
            var r = TermParser + NoneOrMany(Debug("looking for: " + SeperatorChar) + Character(SeperatorChar) + Debug("term time") + TermParser.PassThrough()).Flatten();
            r.Flags = DefaultParserFlags;
            return r;
        }

        #endregion

    }
}
