using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace World
{
	public class settings : RMUD.Settings
	{
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.Perform<PossibleMatch, MudObject>("before command")
                .First
                .Do((match, actor) =>
                    {
                        Console.WriteLine();
                        return SharpRuleEngine.PerformResult.Continue;
                    });

            GlobalRules.Perform<MudObject>("after every command")
                .Last
                .Do((actor) =>
                {
                    Console.WriteLine();
                    return SharpRuleEngine.PerformResult.Continue;
                });

            GlobalRules.Perform<World.Player>("singleplayer game started")
                .First
                .Do((actor) =>
                {
                    Skrogard.Game.SwitchPlayerCharacter(RMUD.MudObject.GetObject("Player"));
                    RMUD.MudObject.Move(Skrogard.Game.Player, RMUD.MudObject.GetObject("Caves.Pod"));
                    RMUD.Core.EnqueuActorCommand(Skrogard.Game.Player, "look");
                    RMUD.Core.ProcessCommands();
                    return SharpRuleEngine.PerformResult.Continue;
                });
        }

        public settings()
        {
            NewPlayerStartRoom = "Caves.Pod";
            PlayerBaseObject = "Player";
            //AmbientExteriorLightingLevel = LightingLevel.Dim;
        }
	}
}
