using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    /// <summary>
    /// This is a fancy door - it can be locked!
    /// 
    /// TODO: Sync locked state with opposite side of portal
    /// </summary>
    /// 

    public static class LockableRules
    {
        public static RuleBuilder<MudObject, MudObject, CheckResult> CheckIsMatchingKey(this MudObject ThisObject)
        {
            return ThisObject.Check<MudObject, MudObject>("is matching key?").ThisOnly(0);
        }

        [RunAtStartup]
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("is matching key?", "Check if [KEY] matches [PORTAL]", "PORTAL", "KEY");

            GlobalRules.Check<MudObject, MudObject, MudObject>("can lock?")
                .When((actor, item, key) => item.GetProperty<bool>("lockable?"))             
                .Do((actor, item, key) =>
                {
                    if (item.GetProperty<bool>("openable?") && item.GetProperty<bool>("open?"))
                    {
                        Core.SendMessage(actor, "@close it first");
                        return CheckResult.Disallow;
                    }

                    if (Core.GlobalRules.ConsiderCheckRuleSilently("is matching key?", item, key) == CheckResult.Disallow)
                    {
                        Core.SendMessage(actor, "@wrong key");
                        return CheckResult.Disallow;
                    }

                    return CheckResult.Allow;
                });

            GlobalRules.Perform<MudObject, MudObject, MudObject>("locked")
                .When((a, b, c) => b.GetProperty<bool>("lockable?"))
                .Do((a, b, c) =>
                {
                    b.SetProperty("locked?", true);
                    return PerformResult.Continue;
                });

            GlobalRules.Perform<MudObject, MudObject, MudObject>("unlocked")
                .When((a, b, c) => b.GetProperty<bool>("lockable?"))
                .Do((a, b, c) =>
               {
                   b.SetProperty("locked?", false);
                   return PerformResult.Continue;
               });

            GlobalRules.Check<MudObject, MudObject>("can open?")
                .First
                .When((a, b) => b.GetProperty<bool>("openable?") && b.GetProperty<bool>("lockable?") && b.GetProperty<bool>("locked?"))
                .Do((a, b) =>
                {
                    Core.SendMessage(a, "@error locked");
                    return CheckResult.Disallow;
                })
                .Name("Can't open locked door rule.");

            GlobalRules.Perform<MudObject, MudObject>("close")
                .When((a, b) => b.GetProperty<bool>("openable?") && b.GetProperty<bool>("lockable?"))
                .Do((a, b) =>
                {
                    b.SetProperty("locked?", false);
                    return PerformResult.Continue;
                });
        }

    }
}
