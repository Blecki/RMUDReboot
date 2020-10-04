public class pit : MudObject
{
	public override void Initialize()
	{
        Room(RoomType.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);

        Short = "Palantine Villa - Pit";

        OpenLink(RMUD.Direction.NORTH, "palantine/antechamber");
	}
}