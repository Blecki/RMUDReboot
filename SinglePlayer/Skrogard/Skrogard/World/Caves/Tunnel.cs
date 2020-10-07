using RMUD;

namespace World.Caves
{

    public class Tunnel : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Tunnel");
            SetProperty("long", "This smooth walled tunnel through the rock is lined on both sides with blue, glowing fungi.");

            OpenLink(Direction.NORTH, "Caves.Charm");
            OpenLink(Direction.SOUTH, "Caves.Glade");
        }
    }
}