using RMUD;

namespace World.Homestead
{

    public class Hallway : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Hallway");
            SetProperty("long", "A narrow hallway runs from the kitchen to a small, sad window. Hardly any light filters in around the metal shutters. There is a door on the north and the south wall.");

            OpenLink(Direction.WEST, "Homestead.Kitchen");
            OpenLink(Direction.NORTH, "Homestead.BoysRoom", Core.GetObject("Homestead.PosterDoor@outside"));
            OpenLink(Direction.SOUTH, "Homestead.GirlsRoom", Core.GetObject("Homestead.FlowerDoor@outside"));
            
        }
    }
}