﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD.Look
{
    internal class LinkRules
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.Perform<MudObject, MudObject>("describe")
                .When((viewer, item) => item.GetProperty<bool>("portal?"))
                .Do((viewer, item) =>
                {
                    if (item.GetProperty<bool>("openable?") && !item.GetProperty<bool>("open?"))
                    {
                        return PerformResult.Stop;
                    }

                    var destination = Core.GetObject(item.GetProperty<String>("link destination"));
                    if (destination == null)
                    {
                        Core.SendMessage(viewer, "@bad link");
                        return PerformResult.Stop;
                    }

                    if (item.GetProperty<bool>("openable?"))
                        Core.SendMessage(viewer, "@through the link you see", item);
                    else
                    {
                        var direction = item.GetProperty<Direction>("link direction");
                        Core.SendMessage(viewer, "@through the cardinal link you see", Link.FriendlyRelativeMessage(direction));
                    }

                    GlobalRules.ConsiderPerformRule("describe locale", viewer, destination);
                    return PerformResult.Continue;
                })
                .Name("Look through a link rule.");
        }
    }
}
