using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace Minimum
{
    public static class Game
    {
        public static RMUD.SinglePlayer.Driver Driver { get; set; }
        internal static MudObject Player { get { return Driver.Player; } }

        public static void SwitchPlayerCharacter(MudObject NewCharacter)
        {
            Driver.SwitchPlayerCharacter(NewCharacter);
        }

        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.Perform<Player>("singleplayer game started")
                .First
                .Do((actor) =>
                {
                    SwitchPlayerCharacter(Core.GetObject("Player"));
                    Core.Move(Player, Core.GetObject("Start"));
                    Core.EnqueuActorCommand(Player, "look");
        
                    return PerformResult.Stop;
                });
        }
    }
}