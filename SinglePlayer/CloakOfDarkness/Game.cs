using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace CloakOfDarkness
{
    public static class Game
    {
        public static RMUD.SinglePlayer.Driver Driver { get; set; }
        internal static RMUD.MudObject Player { get { return Driver.Player; } }

        public static void SwitchPlayerCharacter(RMUD.MudObject NewCharacter)
        {
            Driver.SwitchPlayerCharacter(NewCharacter);
        }

        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            GlobalRules.Perform<Player>("singleplayer game started")
                .First
                .Do((actor) =>
                {
                    SwitchPlayerCharacter(RMUD.MudObject.GetObject("Player"));
                    Core.Move(Player, RMUD.MudObject.GetObject("Foyer"));
                    Core.SendMessage(actor, "Hurrying through the rainswept November night, you're glad to see the bright lights of the Opera House. It's surprising that there aren't more people about but, hey, what do you expect in a cheap demo game...?");
                    Core.EnqueuActorCommand(Player, "look");
                    Core.ProcessCommands();        
                    return PerformResult.Continue;
                });
        }
    }
}