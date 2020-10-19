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

    public static class RegisterLockedDoorProperties
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("locked?", typeof(bool), true, new BoolSerializer());
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("is matching key?", "Check if [KEY] matches [PORTAL]", "PORTAL", "KEY");
        }

        public static RuleBuilder<MudObject, MudObject, CheckResult> CheckIsMatchingKey(this MudObject ThisObject)
        {
            return ThisObject.Check<MudObject, MudObject>("is matching key?").ThisOnly(0);
        }
    }

    public partial class ObjectDecorator
    {
        public static void LockedDoor(MudObject This)
        {
            ObjectDecorator.BasicDoor(This);

            This.SetProperty("lockable?", true); // Ugh - these are declared in StandardActionsModule. Do the door decorators need to go there?

            This.Check<MudObject, MudObject, MudObject>("can lock?").Do((actor, door, key) =>
                {
                    if (This.GetProperty<bool>("open?"))
                    {
                        Core.SendMessage(actor, "@close it first");
                        return CheckResult.Disallow;
                    }

                    if (Core.GlobalRules.ConsiderCheckRuleSilently("is matching key?", door, key) == CheckResult.Disallow)
                    {
                        Core.SendMessage(actor, "@wrong key");
                        return CheckResult.Disallow;
                    }

                    return CheckResult.Allow;
                });

            This.Perform<MudObject, MudObject, MudObject>("locked").Do((a, b, c) =>
                {
                    This.SetProperty("locked", true);
                    return PerformResult.Continue;
                });

            This.Perform<MudObject, MudObject, MudObject>("unlocked").Do((a, b, c) =>
               {
                   b.SetProperty("locked", false);
                   return PerformResult.Continue;
               });

            This.Check<MudObject, MudObject>("can open?")
                .First
                .When((a, b) => b.GetProperty<bool>("locked"))
                .Do((a, b) =>
                {
                    Core.SendMessage(a, "@error locked");
                    return CheckResult.Disallow;
                })
                .Name("Can't open locked door rule.");

            This.Perform<MudObject, MudObject>("close")
                .Do((a, b) => { b.SetProperty("locked", false); return PerformResult.Continue; });
        }

    }
}
