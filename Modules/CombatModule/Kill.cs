using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace CombatModule
{
	internal class Kill : CommandFactory
	{
        public static MatchPreference PreferValidTargets(MudObject Actor, MudObject Object)
        {
            if (Object.HasProperty("health")) return MatchPreference.Likely;
            return MatchPreference.VeryUnlikely;
        }

        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    Or(
                        KeyWord("KILL"),
                        KeyWord("ATTACK")
                    ),
                    BestScore("OBJECT",
                        MustMatch("@kill what",
                            Object("OBJECT", InScope, PreferValidTargets)))))
                .ID("Combat:Kill")
                .Manual("Murder. Death. Kill.")
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
                    // Weapon?
                    if (target.HasProperty("combat_health"))
                    {
                        System.MeleeAttack(actor, target);
                    }
                    else
                    {
                        MudObject.SendMessage(actor, "@you attack uselessly", actor, target);
                        MudObject.SendExternalMessage(actor, "@they attack uselessly", actor, target);
                    }
                    return PerformResult.Continue;
                });
        }
    }
}
