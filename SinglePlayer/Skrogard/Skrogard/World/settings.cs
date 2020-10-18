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
                        return PerformResult.Continue;
                    });

            GlobalRules.Perform<MudObject>("after every command")
                .Last
                .Do((actor) =>
                {
                    Console.WriteLine();
                    return PerformResult.Continue;
                });

            GlobalRules.Perform<World.Player>("singleplayer game started")
                .First
                .Do((actor) =>
                {
                    Skrogard.Game.SwitchPlayerCharacter(Core.GetObject("Player"));
                    Core.Move(Skrogard.Game.Player, Core.GetObject("Caves.Pod"));
                    Core.EnqueuActorCommand(Skrogard.Game.Player, "look");
                    Core.ProcessCommands();
                    return PerformResult.Continue;
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
