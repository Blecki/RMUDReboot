public class garden : RMUD.MudObject
{
	public override void Initialize()
	{
        Room(RoomType.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);
        Short = "Palantine Villa - Garden";

        Move(GetObject("palantine/wolf"), this);

        OpenLink(RMUD.Direction.EAST, "palantine\\antechamber");
        OpenLink(RMUD.Direction.DOWN, "palantine\\cave");
    }
}