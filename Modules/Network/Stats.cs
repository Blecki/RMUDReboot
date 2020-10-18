using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace NetworkModule
{
    public class Stats
    {
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.Perform<MudObject>("enumerate-stats")
                .Do((actor) =>
                {
                    Core.SendMessage(actor, "CLIENTS");
                    return PerformResult.Continue;
                });

            GlobalRules.Perform<MudObject, String>("stats")
                .When((actor, type) => type == "CLIENTS")
                .Do((actor, type) =>
                {
                    Core.SendMessage(actor, "~~ CLIENTS ~~");
                    foreach (var client in Clients.ConnectedClients)
                        if (client is NetworkClient)
                            Core.SendMessage(actor, (client as NetworkClient).ConnectionDescription + (client.Player == null ? "" : (" - " + client.Player.GetProperty<String>("short"))));
                        else
                            Core.SendMessage(actor, "local " + (client.Player == null ? "" : (" - " + client.Player.GetProperty<String>("short"))));
                    return PerformResult.Stop;
                });
        }
    }
}
