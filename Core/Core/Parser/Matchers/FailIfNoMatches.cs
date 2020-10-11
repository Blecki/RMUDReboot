using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher MustMatch(String Message, CommandTokenMatcher Sub)
        {
            return new FailIfNoMatches(Sub, Message);
        }
    }

    internal class FailIfNoMatches : CommandTokenMatcher
    {
        public CommandTokenMatcher Sub;
        public String Message;

        public FailIfNoMatches(CommandTokenMatcher Sub, String Message)
        {
            this.Sub = Sub;
            this.Message = Message;
        }

        override protected List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context)
        {
            var R = new List<PossibleMatch>();
            R.AddRange(Sub.Match(State, Context));
            if (R.Count == 0) throw new CommandParser.MatchAborted(Message);
            return R;
        }

        override public String FindFirstKeyWord() { return Sub.FindFirstKeyWord(); }
        override public String Emit() { return Sub.Emit(); }
    }
}
