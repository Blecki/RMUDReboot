﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class OpenClose : CommandFactory
	{
		public override void Create(CommandParser Parser)
		{
            Parser.AddCommand(
                Sequence(
                    KeyWord("CLOSE"),
                    BestScore("SUBJECT",
                        //MustMatch("@not here",
                            Object("SUBJECT", InScope, (actor, thing) =>
                                {
                                    if (Core.GlobalRules.ConsiderCheckRuleSilently("can close?", actor, thing) == CheckResult.Allow) return MatchPreference.Likely;
                                    return MatchPreference.Unlikely;
                                }).Stage("I got as far as knowing that you wanted to close something, but couldn't tell what."))))
                .ID("StandardActions:Close")
                .Manual("Closes a thing.")
                .Check("can close?", "ACTOR", "SUBJECT")
                .BeforeActing()
                .Perform("close", "ACTOR", "SUBJECT")
                .AfterActing();
		}

        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            Core.StandardMessage("you close", "You close <the0>.");
            Core.StandardMessage("they close", "^<the0> closes <the1>.");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can close?", "[Actor, Item] : Determine if the item can be closed.", "actor", "item");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("close", "[Actor, Item] : Handle the item being closed.", "actor", "item");


            GlobalRules.Perform<MudObject, MudObject>("close").Do((actor, target) =>
            {
                target.SetProperty("open?", false);
                Core.SendMessage(actor, "@you close", target);
                Core.SendExternalMessage(actor, "@they close", actor, target);
                return PerformResult.Continue;
            }).Name("Default close reporting rule.");

            GlobalRules.Check<MudObject, MudObject>("can close?").First.Do((actor, item) => Core.CheckIsVisibleTo(actor, item)).Name("Item must be visible rule.");
        }
    }

    public static class CloseRuleFactoryExtensions
    {
        public static RuleBuilder<MudObject, MudObject, CheckResult> CheckCanClose(this MudObject ThisObject)
        {
            return ThisObject.Check<MudObject, MudObject>("can close?").ThisOnly(1);
        }

        public static RuleBuilder<MudObject, MudObject, PerformResult> PerformClose(this MudObject ThisObject)
        {
            return ThisObject.Perform<MudObject, MudObject>("close").ThisOnly(1);
        }
    }
}
