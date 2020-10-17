using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace CloakOfDarkness
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
        }

        public settings()
        {
            NewPlayerStartRoom = "Foyer";
            PlayerBaseObject = "Player";
        }
	}
}
