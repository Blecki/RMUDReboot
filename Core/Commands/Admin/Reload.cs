﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
    internal class Reload : CommandFactory
    {
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    RequiredRank(500),
                    KeyWord("!RELOAD"),
                    MustMatch("It helps if you give me a path.",
                        Path("TARGET"))))
                .Manual("Given a path, it attempts to recompile that object. The object will be replaced in-place if possible.")
                .ProceduralRule((match, actor) =>
                {
                    var target = match["TARGET"].ToString();
                    var newObject = Core.Database.ReloadObject(target);
                    if (newObject == null) Core.SendMessage(actor, "Failed to reload " + target);
                    else Core.SendMessage(actor, "Reloaded " + target);
                    return PerformResult.Continue;
                });

            Parser.AddCommand(
                 Sequence(
                     RequiredRank(500),
                     KeyWord("!RESET"),
                     MustMatch("It helps if you give me a path.",
                         Path("TARGET"))))
                 .Manual("Given a path, it attempts to reset that object without reloading or recompiling. The object will be replaced in-place if possible.")
                 .ProceduralRule((match, actor) =>
                 {
                     var target = match["TARGET"].ToString();
                     if (Core.Database.ResetObject(target) == null)
                         Core.SendMessage(actor, "Failed to reset " + target);
                     else Core.SendMessage(actor, "Reset " + target);
                     return PerformResult.Continue;
                 });

        }
    }
}