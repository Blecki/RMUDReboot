﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	internal class Whisper : CommandFactory
	{
        public override void Create(CommandParser Parser)
        {
            Parser.AddCommand(
                Sequence(
                    Or(
                        KeyWord("WHISPER"),
                        KeyWord("TELL")),
                    OptionalKeyWord("TO"),
                    MustMatch("Whom?",
                        Object("PLAYER", new ConnectedPlayersObjectSource(), ObjectMatcherSettings.None)),
                    MustMatch("Tell them what?", Rest("SPEECH"))))
                .Manual("Sends a private message to the player of your choice.")
                .ProceduralRule((match, actor) =>
                {
                    if (System.Object.ReferenceEquals(actor, match["PLAYER"]))
                    {
                        Core.SendMessage(actor, "Talking to yourself?");
                        return PerformResult.Stop;
                    }
                    return PerformResult.Continue;
                })
                .ProceduralRule((match, actor) =>
                {
                    var player = match["PLAYER"] as MudObject;
                    Core.SendMessage(player, "[privately " + DateTime.Now + "] ^<the0> : \"" + match["SPEECH"].ToString() + "\"", actor);
                    Core.SendMessage(actor, "[privately to <the0>] ^<the1> : \"" + match["SPEECH"].ToString() + "\"", player, actor);
                    var client = player.GetProperty<Client>("client");
                    if (client is NetworkClient && (client as NetworkClient).IsAfk)
                        Core.SendMessage(actor, "^<the0> is afk : " + player.GetProperty<Account>("account").AFKMessage, player);
                    return PerformResult.Continue;
                });
        }
	}
}
