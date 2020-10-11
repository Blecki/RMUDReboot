using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher Or(params CommandTokenMatcher[] Subs)
        {
            return new Or(Subs);
        }
    }

    public class Or : CommandTokenMatcher
    {
        internal List<CommandTokenMatcher> Matchers = new List<CommandTokenMatcher>();

		public Or(params CommandTokenMatcher[] matchers)
		{
			Matchers.AddRange(matchers);
		}

        override protected List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context)
        {
            var Matches = new List<PossibleMatch>();
            foreach (var Matcher in Matchers)
				Matches.AddRange(Matcher.Match(State, Context));
            return Matches;
        }

        override public String FindFirstKeyWord()
        {
            foreach (var sub in Matchers)
            {
                var s = sub.FindFirstKeyWord();
                if (!String.IsNullOrEmpty(s)) return s;
            }
            return null;
        }

        override public String Emit() { return "( " + String.Join(" | ", Matchers.Select(m => m.Emit())) + " )"; }
    }

}
