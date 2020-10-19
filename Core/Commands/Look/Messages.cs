using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD.Look
{
    internal class Messages
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            Core.StandardMessage("nowhere", "You aren't anywhere.");
            Core.StandardMessage("dark", "It's too dark to see.");
            Core.StandardMessage("also here", "Also here: <l0>.");
            Core.StandardMessage("on which", "(on which is <l0>)");
            Core.StandardMessage("obvious exits", "Obvious exits:");
            Core.StandardMessage("through", "through <the0>");
            Core.StandardMessage("to", "to <the0>");
            Core.StandardMessage("dont see that", "I don't see that here.");
            Core.StandardMessage("cant look relloc", "You can't look <s0> that.");
            Core.StandardMessage("is closed error", "^<the0> is closed.");
            Core.StandardMessage("relloc it is", "^<s0> <the1> is..");
            Core.StandardMessage("nothing relloc it", "There is nothing <s0> <the1>.");
            Core.StandardMessage("through the link you see", "Through <the0> you see...");
            Core.StandardMessage("through the cardinal link you see", "^<s0> you see...");
        }
    }
}
