﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
    internal class Inventory : CommandFactory
    {
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Or(
                    KeyWord("INVENTORY"),
                    KeyWord("INV"),
                    KeyWord("I")))
                .ID("StandardActions:Inventory")
                .Manual("Displays what you are wearing and carrying.")
                .Perform("inventory", "ACTOR");
        }

        public static void AtStartup(RuleEngine GlobalRules)
        {
            Core.StandardMessage("carrying", "You are carrying..");

            GlobalRules.DeclarePerformRuleBook<MudObject>("inventory", "[Actor] : Describes a player's inventory to themselves.", "actor");

            GlobalRules.Perform<MudObject>("inventory")
                .Do(a =>
                {
                    var heldObjects = a.GetContents(RelativeLocations.HELD);
                    if (heldObjects.Count == 0) Core.SendMessage(a, "@empty handed", a);
                    else
                    {
                        Core.SendMessage(a, "@carrying");
                        foreach (var item in heldObjects)
                            Core.SendMessage(a, "  <a0>", item);
                    }
                    return PerformResult.Continue;
                })
                .Name("List held items in inventory rule.");
        }
    }
}
