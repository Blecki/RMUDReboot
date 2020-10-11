using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class CommandFactory
    {
        public static CommandTokenMatcher SetValue(String Name, Object Value)
        {
            return new SetValue(Name, Value);
        }
    }

    internal class SetValue : CommandTokenMatcher
    {
        public String Name;
        public Object Value;

        public SetValue(String Name, Object Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        override protected List<PossibleMatch> ImplementMatch(PossibleMatch State, MatchContext Context)
        {
            return new List<PossibleMatch>() { State.With(Name, Value) };
        }

        override public String FindFirstKeyWord() { return null; }
        override public String Emit() { return "[SET " + (Value == null ? "%NULL" : Value.ToString()) + " => '" + Name + "']"; }
    }
}
