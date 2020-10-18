using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace AdminModule
{
    internal class Scope : CommandFactory
    {
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    RequiredRank(500),
                    KeyWord("!SCOPE")))
                .Manual("List all of the objects in scope")
                .ProceduralRule((match, actor) =>
                {
                    foreach (var thing in Core.EnumerateVisibleTree(Core.FindLocale(actor)))
                        Core.SendMessage(actor, thing.GetProperty<String>("short") + " - " + thing.GetType().Name);
                    return PerformResult.Continue;
                }, "List all the damn things in scope rule.");
        }
    }
}
