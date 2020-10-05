using RMUD;

namespace World.Surface
{

    public class Homestead : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Homestead");
            SetProperty("long", "A homestead is built under the massive boulder. Made of cobbled together scrap, no two parts match. Thick shutters cover windows that aren't quite square. One window stands open, glass shattered and scattered across the ground.");

            OpenLink(Direction.SOUTH, "Surface.Badlands");
            OpenLink(Direction.NORTH, "Surface.Overlook");
            OpenLink(Direction.IN, "Homestead.Kitchen");

            Move(new MudObject("glass shards", "These long shards of dirty glass are quite sharp. Be careful."), this, RelativeLocations.Contents);
        }
    }
}