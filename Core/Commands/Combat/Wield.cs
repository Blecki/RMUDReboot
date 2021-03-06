﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class Wield : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    KeyWord("WIELD"),
                    BestScore("OBJECT",
                        MustMatch("@kill what",
                            Object("OBJECT", InScope, PreferHeld)))))
                .ID("Combat:Wield")
                .Manual("Arm thyself.")
                .Check("can wield?", "ACTOR", "OBJECT")
                .BeforeActing()
                .Perform("wield", "ACTOR", "OBJECT")
                .AfterActing();
        }

        [RunAtStartup]
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.DeclareCheckRuleBook<MudObject, MudObject>("can wield?", "[Actor, Item] : Can the actor wield the item?", "actor", "item");
            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("wield", "[Actor, Item] : Handle the actor wielding the item.", "actor", "item");

            GlobalRules.Check<MudObject, MudObject>("can wield?").Do((a, b) => CheckResult.Allow).Name("Improvised weapon rule.");

            GlobalRules.Check<MudObject, MudObject>("can wield?")
                .When((a, b) => !Core.ObjectContainsObject(a, b))
                .Do((actor, item) =>
                {
                    Core.SendMessage(actor, "@dont have that");
                    return CheckResult.Disallow;
                })
                .Name("Can't wield what you don't have rule.");

            GlobalRules.Perform<MudObject, MudObject>("wield").Do((actor, target) =>
                {
                    actor.SetProperty("combat weapon", target);
                    Core.SendMessage(actor, "@you wield", actor, target);
                    Core.SendExternalMessage(actor, "@they wield", actor, target);
                    return PerformResult.Continue;
                });
        }
    }
}
