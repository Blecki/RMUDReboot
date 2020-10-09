using RMUD;

namespace World.Wilderness
{
    public class Slope : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Slope");
            SetProperty("long", "The path slopes steeply between two large stands of thorn bushes, from a clearing above to the bottom of a narrow ravine.");

            OpenLink(Direction.WEST, "Wilderness.Ravine");
            OpenLink(Direction.EAST, "Wilderness.Clearing");
        }
    }
}