public class platos_closet : MudObject
{
	public override void Initialize()
	{
        Room(RoomType.Interior);
        SetProperty("ambient light", LightingLevel.Bright);

        Short = "Palantine Villa - Plato's Closet";

        AddScenery(new lamp());

        Move(ClothingFactory.Create("pair of jeans", ClothingLayer.Outer, ClothingBodyPart.Legs), this);
        Move(ClothingFactory.Create("polo shirt", ClothingLayer.Outer, ClothingBodyPart.Torso), this);
        Move(ClothingFactory.Create("pair of briefs", ClothingLayer.Under, ClothingBodyPart.Legs), this);

        OpenLink(Direction.WEST, "palantine\\solar");
	}
}

public class lamp : MudObject
{
    public lamp()
    {
        AddNoun("gas", "lamp");
        Long = "This little gas lamp somehow manages to fill the endless closet with light.";

        Value<MudObject, LightingLevel>("light level").Do(a => LightingLevel.Bright);

        SetProperty("scenery?", true);
    }

}