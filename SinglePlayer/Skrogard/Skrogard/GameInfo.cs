using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimum
{
    public class GameInfo : RMUD.SinglePlayer.GameInfo
    {
        public GameInfo()
        {
            Title = "Skrogard";
            DatabaseNameSpace = "World";
            Description = "The Skrogard setting for NaMuBuMo.";
            Modules = new List<string>(new String[] { "StandardActionsModule.dll", "ConversationModule.dll", "QuestModule.dll", "AliasModule.dll", "IntroductionModule.dll", "AdminModule.dll", "ClothingModule.dll", "CombatModule.dll" });
        }
    }
}
