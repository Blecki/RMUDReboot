using RMUD;
using static RMUD.Core;

public class dark_room : MudObject
{
	public override void Initialize()
	{
        Locale(RMUD.Locale.Interior);
        SetProperty("ambient light", LightingLevel.Dark);
        Short = "Palantine Villa - Soul Chamber";
        Long = "It does not matter how bright a light you carry, it cannot banish the shadows from your soul.";

        OpenLink(Direction.WEST, "palantine\\disambig", GetObject("palantine\\disambig_red_door@outside"));
	}
}
