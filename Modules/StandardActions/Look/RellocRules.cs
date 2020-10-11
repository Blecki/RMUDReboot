using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace StandardActionsModule.Look
{
    internal class RellocRules
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject, RelativeLocations>("can look relloc?", "[Actor, Item, Relative Location] : Can the actor look in/on/under/behind the item?", "actor", "item", "relloc");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .Do((actor, item, relloc) => MudObject.CheckIsVisibleTo(actor, item))
                .Name("Container must be visible rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .When((actor, item, relloc) => (item.LocationsSupported & relloc) != relloc)
                .Do((actor, item, relloc) =>
                {
                    MudObject.SendMessage(actor, "@cant look relloc", Relloc.GetRelativeLocationName(relloc));
                    return SharpRuleEngine.CheckResult.Disallow;
                })
                .Name("Container must support relloc rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .When((actor, item, relloc) => (relloc == RelativeLocations.In) && !item.GetProperty<bool>("open?"))
                .Do((actor, item, relloc) =>
                {
                    MudObject.SendMessage(actor, "@is closed error", item);
                    return SharpRuleEngine.CheckResult.Disallow;
                })
                .Name("Container must be open to look in rule.");

            GlobalRules.Check<MudObject, MudObject, RelativeLocations>("can look relloc?")
                .Do((actor, item, relloc) => SharpRuleEngine.CheckResult.Allow)
                .Name("Default allow looking relloc rule.");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject, RelativeLocations>("look relloc", "[Actor, Item, Relative Location] : Handle the actor looking on/under/in/behind the item.", "actor", "item", "relloc");

            GlobalRules.Perform<MudObject, MudObject, RelativeLocations>("look relloc")
                .Do((actor, item, relloc) =>
                {
                    var contents = new List<MudObject>(item.EnumerateObjects(relloc));

                    if (contents.Count > 0)
                    {
                        MudObject.SendMessage(actor, "@relloc it is", Relloc.GetRelativeLocationName(relloc), item);
                        foreach (var thing in contents)
                            MudObject.SendMessage(actor, "  <a0>", thing);
                    }
                    else
                        MudObject.SendMessage(actor, "@nothing relloc it", Relloc.GetRelativeLocationName(relloc), item);

                    return SharpRuleEngine.PerformResult.Continue;
                })
                .Name("List contents in relative location rule.");
        }
    }
}
