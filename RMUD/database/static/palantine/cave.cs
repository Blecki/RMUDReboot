public class cave : MudObject
{
	public override void Initialize()
	{
        Room(RoomType.Exterior);
        SetProperty("ambient light", LightingLevel.Bright);
        Short = "Palantine Villa - Cave";

        OpenLink(Direction.UP, "palantine\\garden");
	}
}