using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrogard
{
    public static class Game
    {
        public static RMUD.SinglePlayer.Driver Driver { get; set; }
        internal static RMUD.MudObject Player { get { return Driver.Player; } }

        public static void SwitchPlayerCharacter(RMUD.MudObject NewCharacter)
        {
            Driver.SwitchPlayerCharacter(NewCharacter);
        }
    }
}