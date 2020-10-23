using RMUD;
using static RMUD.Core;

public class garden : RMUD.MudObject
{
	public override void Initialize()
	{
        Locale(RMUD.Locale.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);
        Short = "Palantine Villa - Garden";

        Move(GetObject("palantine/wolf"), this);

        OpenLink(RMUD.Direction.EAST, "palantine\\antechamber");
        OpenLink(RMUD.Direction.DOWN, "palantine\\cave");
    }
}