﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher SingleWord(String ArgumentName)
        {
            return new SingleWord(ArgumentName);
        }
    }

    internal class SingleWord : CommandTokenMatcher
    {
        public String ArgumentName;

        internal SingleWord(String ArgumentName)
        {
			this.ArgumentName = ArgumentName;
        }

        public List<PossibleMatch> Match(PossibleMatch State, MatchContext Context)
        {
            var r = new List<PossibleMatch>();
            if (State.Next != null)
                r.Add(State.AdvanceWith(ArgumentName, State.Next.Value));
			return r;
        }

        public String FindFirstKeyWord() { return null; }
		public String Emit() { return "[WORD => '" + ArgumentName + "']"; }
    }
}
