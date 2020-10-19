using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class Wear : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    KeyWord("WEAR"),
                    BestScore("OBJECT",
                        MustMatch("@clothing wear what",
                            Object("OBJECT", InScope, PreferHeld)))))
                .ID("Clothing:Wear")
                .Manual("Cover your disgusting flesh.")
                .Check("can wear?", "ACTOR", "OBJECT")
                .BeforeActing()
                .Perform("worn", "ACTOR", "OBJECT")
                .AfterActing();
        }

        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can wear?", "[Actor, Item] : Can the actor wear the item?", "actor", "item");
            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("worn", "[Actor, Item] : Handle the actor wearing the item.", "actor", "item");

            GlobalRules.Check<MudObject, MudObject>("can wear?")
                .When((a, b) => !Core.ObjectContainsObject(a, b))
                .Do((actor, item) =>
                {
                    Core.SendMessage(actor, "@dont have that");
                    return CheckResult.Disallow;
                });

            GlobalRules.Check<MudObject, MudObject>("can wear?")
                .When((a, b) => a.RelativeLocationOf(b) == RelativeLocations.WORN)
                .Do((a, b) =>
                {
                    Core.SendMessage(a, "@clothing already wearing");
                    return CheckResult.Disallow;
                });

            GlobalRules.Check<MudObject, MudObject>("can wear?")
                .When((actor, item) => !item.GetProperty<bool>("wearable?"))
                .When((actor, item) => !actor.GetProperty<bool>("actor?"))
                .Do((actor, item) =>
                {
                    Core.SendMessage(actor, "@clothing cant wear");
                    return CheckResult.Disallow;
                })
                .Name("Can't wear unwearable things rule.");

            GlobalRules.Check<MudObject, MudObject>("can wear?").Do((a, b) => CheckResult.Allow);

            GlobalRules.Perform<MudObject, MudObject>("worn").Do((actor, target) =>
                {
                    Core.SendMessage(actor, "@clothing you wear", target);
                    Core.SendExternalMessage(actor, "@clothing they wear", actor, target);
                    Core.Move(target, actor, RelativeLocations.WORN);
                    return PerformResult.Continue;
                });
        }
    }
}
