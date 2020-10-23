using RMUD;
using static RMUD.Core;

public class cave : MudObject
{
	public override void Initialize()
	{
        Locale(RMUD.Locale.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);
        Short = "Palantine Villa - Cave";

        OpenLink(Direction.UP, "palantine\\garden");
	}
}