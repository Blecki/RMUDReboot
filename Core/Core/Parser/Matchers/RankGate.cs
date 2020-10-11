using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher RequiredRank(int Rank)
        {
            return new RankGate(Rank);
        }
    }

    internal class RankGate : CommandTokenMatcher
    {
        public int RequiredRank;

		internal RankGate(int RequiredRank)
		{
			this.RequiredRank = RequiredRank;
		}

        override protected List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context)
        {
            var R = new List<PossibleMatch>();
            if (Context.ExecutingActor.GetProperty<int>("rank") >= RequiredRank)
                R.Add(State);
            //else
            //    throw new CommandParser.MatchAborted("You do not have sufficient rank to use that command.");
			return R;
        }

        override public String FindFirstKeyWord() { return null; }
        override public String Emit() { return "<Rank must be >= " + RequiredRank + ">"; }
    }
}
