using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
    internal class Quest : CommandFactory
    {
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                 KeyWord("QUEST"))
                .Name("QUEST")
                .Manual("See what your active quest is.")
                .ProceduralRule((match, actor) =>
                {
                    if (actor.GetProperty<MudObject>("active-quest") == null)
                        Core.SendMessage(actor, "You do not have an active quest.");
                    else
                        Core.SendMessage(actor, "Active quest: <a0>.", actor.GetProperty<MudObject>("active-quest"));
                    return PerformResult.Continue;
                }, "display the active quest rule.");
        }
    }
}