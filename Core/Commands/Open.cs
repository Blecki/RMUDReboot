﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class Open : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    KeyWord("OPEN"),
                    BestScore("SUBJECT",
                        MustMatch("@not here",
                            Object("SUBJECT", InScope, (actor, thing) =>
                            {
                                if (Core.GlobalRules.ConsiderCheckRuleSilently("can open?", actor, thing) == CheckResult.Allow) return MatchPreference.Likely;
                                return MatchPreference.Unlikely;
                            })))))
                .ID("StandardActions:Open")
                .Manual("Opens an openable thing.")
                .Check("can open?", "ACTOR", "SUBJECT")
                .BeforeActing()
                .Perform("opened", "ACTOR", "SUBJECT")
                .AfterActing();
        }

        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {

            Core.StandardMessage("not openable", "I don't think the concept of 'open' applies to that.");
            Core.StandardMessage("you open", "You open <the0>.");
            Core.StandardMessage("they open", "^<the0> opens <the1>.");

            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can open?", "[Actor, Item] : Can the actor open the item?", "actor", "item");

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("opened", "[Actor, Item] : Handle the actor opening the item.", "actor", "item");

           

            GlobalRules.Perform<MudObject, MudObject>("opened").Do((actor, target) =>
            {
                target.SetProperty("open?", true);
                Core.SendMessage(actor, "@you open", target);
                Core.SendExternalMessage(actor, "@they open", actor, target);
                return PerformResult.Continue;
            }).Name("Default report opening rule.");

            GlobalRules.Check<MudObject, MudObject>("can open?").First.Do((actor, item) => Core.CheckIsVisibleTo(actor, item)).Name("Item must be visible rule.");
        }
    }

    public static class OpenExtensions
    {
        public static RuleBuilder<MudObject, MudObject, PerformResult> PerformOpened(this MudObject Object)
        {
            return Object.Perform<MudObject, MudObject>("opened").ThisOnly(1);
        }

        public static RuleBuilder<MudObject, MudObject, CheckResult> CheckCanOpen(this MudObject Object)
        {
            return Object.Check<MudObject, MudObject>("can open?").ThisOnly(1);
        }
    }
}
