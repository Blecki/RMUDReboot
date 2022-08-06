using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
    // Todo: Get spellbook from actor somehow.
    public class SpellbookObjectSource : IObjectSource
    {
        public List<MudObject> GetObjects(PossibleMatch State, MatchContext Context)
        {
            return new List<MudObject>(Context.ObjectsInScope);
        }
    }

    internal class Cast : CommandFactory
	{
        // Todo: Spells can be cast one someone, but the target is optional
        // In rules - automatically handle cases where we are in combat vs not; if it's an offensive spell it needs to invoke the combat system when it's targetted at a creature and we aren't already in combat.

        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    Or(
                        KeyWord("CAST")
                    ),
                    BestScore("OBJECT",
                        MustMatch("@kill what",
                            Object("OBJECT", new SpellbookObjectSource())))))
                .ID("Combat:Cast")
                .Manual("Cast a spell")
                .Check("can attack?", "ACTOR", "OBJECT")
                .BeforeActing()
                .Perform("attack", "ACTOR", "OBJECT")
                .AfterActing();
        }

        [RunAtStartup]
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can attack?", "[Actor, Other] : Can the actor the other?", "actor", "other");
            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("attack", "[Actor, Other] : Handle the actor the other.", "actor", "other");

            GlobalRules.Check<MudObject, MudObject>("can attack?").Do((a, b) => CheckResult.Allow).Name("Can attack anything by default rule.");

            GlobalRules.Perform<MudObject, MudObject>("attack").Do((actor, target) =>
                {
                    if (target.HasProperty("combat health"))
                    {
                        actor.SetProperty("combat target", target);
                    }
                    else
                    {
                        Core.SendMessage(actor, "@you attack uselessly", actor, target);
                        Core.SendExternalMessage(actor, "@they attack uselessly", actor, target);
                    }
                    return PerformResult.Continue;
                });
        }
    }
}
