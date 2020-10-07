using RMUD;

namespace World.Caves
{

    public class Passage : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Passage");
            SetProperty("long", "This narrow passage winds through the rock. Shallow water splashes underfoot.");

            AddScenery("The pod is beat up and a bit rusty. It looks very old.", "pod");

            OpenLink(Direction.EAST, "Homestead.Workshop", GetObject("Homestead.WorkshopDoor@inside"));
            OpenLink(Direction.WEST, "Caves.Lake");
        }
    }
}