using RMUD;

namespace World.Caves
{

    public class Lake : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Lake");
            SetProperty("long", "A small body of still water marks the boundary between stone and scrap. A ledge circumnavigates the chamber, allowing you to cross without getting wet.");

            OpenLink(Direction.WEST, "Caves.ExcavatorCave");
            OpenLink(Direction.EAST, "Caves.Passage");
            OpenLink(Direction.SOUTH, "Caves.Crevice");
        }
    }
}