﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class Save : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    RequiredRank(500),
                    KeyWord("!SAVE")))
                .Manual("Saves all persistent objects to disc.")
                .ProceduralRule((match, actor) =>
                {
                    Core.CommandTimeoutEnabled = false;

                    //TODO MudObject.SendGlobalMessage("The database is being saved. There may be a brief delay.");
                    Core.SendPendingMessages();

                    var saved = Core.Database.Save();

                    //TODO MudObject.SendGlobalMessage("The database has been saved.");
                    Core.SendMessage(actor, String.Format("I saved {0} persistent objects.", saved));
                    return PerformResult.Continue;
                });
		}
	}

}
