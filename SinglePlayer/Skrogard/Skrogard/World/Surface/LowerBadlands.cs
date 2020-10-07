using RMUD;

namespace World.Surface
{

    public class LowerBadlands : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Lower Badlands");
            SetProperty("long", "Small boulders and lose rocks dot the scrub land at the bottom of the foothills. A stream cuts across the plain to the east.");

            OpenLink(Direction.EAST, "Surface.Bridge");
            OpenLink(Direction.NORTH, "Surface.Badlands");
        }
    }
}