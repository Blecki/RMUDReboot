using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public class CommandTokenMatcher
    {
        private String ParseStageDescription;

        public CommandTokenMatcher Stage(String Description)
        {
            this.ParseStageDescription = Description;
            return this;
        }


        /// <summary>
        /// Find all possible matches. Matches returned should be ordered best to worst.
        /// </summary>
        /// <param name="State"></param>
        /// <param name="Context"></param>
        /// <returns>An empty list if no match, otherwise a list of possible matches</returns>
        public List<PossibleMatch> Match(PossibleMatch State, MatchContext Context)
        {
            Context.ParseStage(State, ParseStageDescription);
            return ImplementMatch(State, Context);
        }

        /// <summary>
        /// Find all possible matches. Matches returned should be ordered best to worst.
        /// </summary>
        /// <param name="State"></param>
        /// <param name="Context"></param>
        /// <returns>An empty list if no match, otherwise a list of possible matches</returns>
        protected virtual List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context) { throw new NotImplementedException(); }

        /// <summary>
        /// Find the first KeyWord matcher in the matcher tree, or null, if no KeyWord matcher can be found.
        /// </summary>
        /// <returns>Null, or the word matched by the first KeyWord matcher in the matcher tree.</returns>
        public virtual String FindFirstKeyWord() { throw new NotImplementedException(); }

        /// <summary>
        /// Format this matcher in a friendly string.
        /// </summary>
        /// <returns>The string description of this matcher</returns>
		public virtual String Emit() { throw new NotImplementedException(); }
    }
}
