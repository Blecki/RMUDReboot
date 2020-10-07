using RMUD;

namespace World.Caves
{

    public class Crevice : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Crevice");
            SetProperty("long", "A narrow crevice slants steeply downward, metal on one side and stone on the other. One shift in the junk could close this passage, but for now it remains a small gap just large enough to wiggle through.");

            OpenLink(Direction.NORTH, "Caves.Lake");
            OpenLink(Direction.WEST, "Caves.Chasm");
        }
    }
}