﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMUD
{
    /// <summary>
    /// Anything that might supply rules for consideration must implement RuleSource. Any object implementing RuleSource
    /// passed as an argument to a consider or value rule function will have it's rules considered when executing the
    /// rulebook.
    /// </summary>
    public partial class MudObject
    {
        public RuleSet Rules { get; private set; }

        /// <summary>
        /// Another rule source that this rule source is related to. After all rule source arguments to a rulebook have
        /// been examined for applicable rules, their linked rule sources will be examined. This mechanism is what 
        /// allows rooms to define rules that affect actions that only involve their contents. MudObject's are implemented
        /// such that their location is their linked rule source.
        /// </summary>
        public virtual MudObject LinkedRuleSource { get { return null; } }

        public virtual RuleEngine GlobalRules { get { return Core.GlobalRules; } }

        public PerformResult ConsiderPerformRule(String Name, params Object[] Arguments)
        {
            if (GlobalRules != null)
                return GlobalRules.ConsiderPerformRule(Name, Arguments);
            return PerformResult.Continue;
        }

        public CheckResult ConsiderCheckRule(String Name, params Object[] Arguments)
        {
            if (GlobalRules != null)
                return GlobalRules.ConsiderCheckRule(Name, Arguments);
            return CheckResult.Allow;
        }

        public RT ConsiderValueRule<RT>(String Name, params Object[] Arguments)
        {
            if (GlobalRules != null)
                return GlobalRules.ConsiderValueRule<RT>(Name, Arguments);
            return default(RT);
        }

        public void DeleteRule(String RuleBookName, String RuleID)
        {
            if (Rules != null) Rules.DeleteRule(RuleBookName, RuleID);
        }

        public void DeleteAllRules(String RuleID)
        {
            if (Rules != null) Rules.DeleteAll(RuleID);
        }
    }
}
