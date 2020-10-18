using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace NetworkModule
{
	internal class Who : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                KeyWord("WHO"))
                .Manual("Displays a list of current logged in players.")
                .ProceduralRule((match, actor) =>
                {
                    var clients = Clients.ConnectedClients.Where(c => c is NetworkClient && (c as NetworkClient).IsLoggedOn);
                    Core.SendMessage(actor, "~~ THESE PLAYERS ARE ONLINE NOW ~~");
                    foreach (NetworkClient client in clients)
                        Core.SendMessage(actor,
                            "[" + Core.SettingsObject.GetNameForRank(client.Player.GetProperty<int>("rank")) + "] <a0> ["
                            + client.ConnectionDescription + "]"
                            + (client.IsAfk ? (" afk: " + client.Player.GetProperty<Account>("account").AFKMessage) : "")
                            + (client.Player.Location.HasValue(out var loc) ? (" -- " + loc.Path) : ""),
                            client.Player);
                    return PerformResult.Continue;
                });
        }
	}
}
