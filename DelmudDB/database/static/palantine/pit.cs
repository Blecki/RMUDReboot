using RMUD;
using static RMUD.Core;

public class pit : MudObject
{
	public override void Initialize()
	{
        Locale(RMUD.Locale.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);

        Short = "Palantine Villa - Pit";

        OpenLink(RMUD.Direction.NORTH, "palantine/antechamber");
	}
}