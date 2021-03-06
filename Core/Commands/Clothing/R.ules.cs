﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace RMUD
{
    public class ClothingRules 
    {
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.Perform<MudObject>("inventory")
                .Do(a =>
                {
                    var wornObjects = a.GetContents(RelativeLocations.WORN);
                    if (wornObjects.Count == 0) Core.SendMessage(a, "@nude");
                    else
                    {
                        Core.SendMessage(a, "@clothing wearing");
                        foreach (var item in wornObjects)
                            Core.SendMessage(a, "  <a0>", item);
                    }
                    return PerformResult.Continue;
                })
                .Name("List worn items in inventory rule.");

            GlobalRules.Check<MudObject, MudObject>("can wear?")
                .When((actor, item) => actor.GetProperty<bool>("actor?"))
                .Do((actor, item) =>
                {
                    var layer = item.GetProperty<ClothingLayer>("clothing layer");
                    var part = item.GetProperty<ClothingBodyPart>("clothing part");
                    foreach (var wornItem in actor.EnumerateObjects(RelativeLocations.WORN))
                        if (wornItem.GetProperty<ClothingLayer>("clothing layer") == layer && wornItem.GetProperty<ClothingBodyPart>("clothing part") == part)
                        {
                            Core.SendMessage(actor, "@clothing remove first", wornItem);
                            return CheckResult.Disallow;
                        }
                    return CheckResult.Continue;
                })
                .Name("Check clothing layering before wearing rule.");

            GlobalRules.Check<MudObject, MudObject>("can remove?")
                .Do((actor, item) =>
                {
                    var layer = item.GetProperty<ClothingLayer>("clothing layer");
                    var part = item.GetProperty<ClothingBodyPart>("clothing part");
                    foreach (var wornItem in actor.EnumerateObjects(RelativeLocations.WORN))
                        if (wornItem.GetProperty<ClothingLayer>("clothing layer") < layer && wornItem.GetProperty<ClothingBodyPart>("clothing part") == part)
                        {
                            Core.SendMessage(actor, "@clothing remove first", wornItem);
                            return CheckResult.Disallow;
                        }
                    return CheckResult.Allow;
                })
                .Name("Can't remove items under other items rule.");

            
            GlobalRules.Perform<MudObject, MudObject>("describe")
                .First
                .When((viewer, actor) => actor.GetProperty<bool>("actor?"))
                .Do((viewer, actor) =>
                {
                    var wornItems = actor.GetContents(RelativeLocations.WORN);
                    if (wornItems.Count == 0)
                        Core.SendMessage(viewer, "@clothing they are nude", actor);
                    else
                        Core.SendMessage(viewer, "@clothing they are wearing", actor, wornItems);
                    return PerformResult.Continue;
                })
                .Name("List worn items when describing an actor rule.");
        }
    }
}
