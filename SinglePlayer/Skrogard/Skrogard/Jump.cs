using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace World
{
	internal class Jump : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(KeyWord("JUMP"))
                .ID("Skogard:Jump")
                .Manual("Why not jump?")
                .Perform("jump", "ACTOR");
        }

        public static void AtStartup(RuleEngine GlobalRules)
        {
            Core.StandardMessage("jump", "^<the0> jumps around.");

            GlobalRules.DeclarePerformRuleBook<MudObject, String>("jump", "[Actor] : Handle the actor jumping.");

            GlobalRules.Perform<MudObject>("jump")
                .Do((actor) =>
                {
                    MudObject.SendExternalMessage(actor, "@jump", actor);
                    MudObject.SendMessage(actor, "You jump about.");
                    return PerformResult.Continue;
                })
                .Name("Default jump rule.");
        }
    }
}
