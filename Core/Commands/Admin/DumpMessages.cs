﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class DumpMessages : CommandFactory
	{
		public override void Create(CommandParser Parser)
		{
            Parser.AddCommand(
                KeyWord("!DUMPMESSAGES"))
                .Manual("Dump defined messages to messages.txt")
                .ProceduralRule((match, actor) =>
                {
                    var builder = new StringBuilder();
                    Core.DumpMessagesForCustomization(builder);
                    System.IO.File.WriteAllText("messages.txt", builder.ToString());
                    Core.SendMessage(actor, "Messages dumped to messages.txt.");
                    return PerformResult.Continue;
                });
		}
	}
}
