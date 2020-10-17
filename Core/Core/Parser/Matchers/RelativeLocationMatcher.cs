using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher RelativeLocation(String CaptureName)
        {
            return new RelativeLocationMatcher(CaptureName);
        }
    }

    internal class RelativeLocationMatcher : CommandTokenMatcher
    {
        public String ArgumentName;

        internal RelativeLocationMatcher(String ArgumentName)
        {
			this.ArgumentName = ArgumentName;
        }

        override protected List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context)
        {
            var r = new List<PossibleMatch>();
			if (State.Next == null) return r;

            var word = State.Next.Value.ToUpper();
            if (word == "ON")
                r.Add(State.AdvanceWith(ArgumentName, RelativeLocations.ON));
            else  if (word == "IN")
                r.Add(State.AdvanceWith(ArgumentName, RelativeLocations.IN));
            else if (word == "UNDER")
                r.Add(State.AdvanceWith(ArgumentName, RelativeLocations.UNDER));
            else if (word == "BEHIND")
                r.Add(State.AdvanceWith(ArgumentName, RelativeLocations.BEHIND));
			return r;
        }

        override public String FindFirstKeyWord() { return null; }
        override public String Emit() { return "[RELLOC => '" + ArgumentName + "']"; }
    }
}
