using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public class OpenableRules
    {
        [RunAtStartup]
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.Check<MudObject, MudObject>("can open?")
               .When((actor, item) => !item.GetProperty<bool>("openable?"))
               .Do((a, b) =>
               {
                   Core.SendMessage(a, "@not openable");
                   return CheckResult.Disallow;
               })
               .Name("Can't open the unopenable rule.");

            GlobalRules.Check<MudObject, MudObject>("can open?")
                .When((actor, item) => item.GetProperty<bool>("openable?") && item.GetProperty<bool>("open?"))
                .Do((a, b) =>
                {
                        Core.SendMessage(a, "@already open");
                        return CheckResult.Disallow;
                })
                .Name("Already open rule.");

            GlobalRules.Check<MudObject, MudObject>("can open?")
                .Last
                .When((actor, item) => item.GetProperty<bool>("openable?"))
                .Do((a, b) => CheckResult.Allow)
                .Name("Default go ahead and open it rule.");


            GlobalRules.Check<MudObject, MudObject>("can close?")
               .When((actor, item) => !item.GetProperty<bool>("openable?"))
               .Do((a, b) =>
               {
                   Core.SendMessage(a, "@not openable");
                   return CheckResult.Disallow;
               })
               .Name("Can't close the unopenable rule.");

            GlobalRules.Check<MudObject, MudObject>("can close?")
                .When((actor, item) => item.GetProperty<bool>("openable?") && !item.GetProperty<bool>("open?"))
                .Do((a, b) =>
                {
                    Core.SendMessage(a, "@already closed");
                    return CheckResult.Disallow;
                })
                .Name("Already closed rule.");

            GlobalRules.Check<MudObject, MudObject>("can close?")
                .Last
                .When((actor, item) => item.GetProperty<bool>("openable?"))
                .Do((a, b) => CheckResult.Allow)
                .Name("Default go ahead and close it rule.");


            GlobalRules.Perform<MudObject, MudObject>("opened")
                .When((actor, item) => item.GetProperty<bool>("portal?") && item.GetProperty<bool>("openable?"))
                .Do((actor, item) =>
                {
                    // Doors are usually two-sided. If there is an opposite side, we need to open it and emit appropriate
                    // messages.
                    var otherSide = Core.FindOppositeSide(item);
                    if (otherSide != null)
                    {
                        otherSide.SetProperty("open?", true);

                        // This message is defined in the standard actions module. It is perhaps a bit coupled?
                        Core.SendLocaleMessage(otherSide, "@they open", actor, item);
                        Core.MarkLocaleForUpdate(otherSide);
                    }

                    return PerformResult.Continue;
                })
                .Name("Open other side of portal rule.");

            GlobalRules.Perform<MudObject, MudObject>("close")
                .When((actor, item) => item.GetProperty<bool>("portal?") && item.GetProperty<bool>("openable?"))
                .Do((actor, item) =>
                {

                    // Doors are usually two-sided. If there is an opposite side, we need to close it and emit
                    // appropriate messages.
                    var otherSide = Core.FindOppositeSide(item);
                    if (otherSide != null)
                    {
                        otherSide.SetProperty("open?", false);
                        Core.SendLocaleMessage(otherSide, "@they close", actor, item);
                        Core.MarkLocaleForUpdate(otherSide);
                    }

                    return PerformResult.Continue;
                })
                .Name("Close other side of portal rule");

            GlobalRules.Check<MudObject, MudObject>("can go?")
                .When((actor, link) => link != null && link.GetProperty<bool>("openable?") && !link.GetProperty<bool>("open?"))
                .Do((actor, link) =>
                {
                    Core.SendMessage(actor, "@first opening", link);
                    var tryOpen = Core.Try("StandardActions:Open", Core.ExecutingCommand.With("SUBJECT", link), actor);
                    if (tryOpen == PerformResult.Stop)
                        return CheckResult.Disallow;
                    return CheckResult.Continue;
                })
                .Name("Try opening a closed door before going rule.");
        }
    }
}
