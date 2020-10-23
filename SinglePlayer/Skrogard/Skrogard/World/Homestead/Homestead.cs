using RMUD;

namespace World.Homestead
{

    public class Homestead : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Homestead");
            SetProperty("long", "A homestead is built under the massive boulder. Made of cobbled together scrap, no two parts match. Thick shutters cover windows that aren't quite square. One window stands open, glass shattered and scattered across the ground.");

            OpenLink(Direction.SOUTH, "Surface.Badlands");
            OpenLink(Direction.NORTH, "Surface.Overlook");
            OpenLink(Direction.EAST, "Homestead.Ledge");
            OpenLink(Direction.IN, "Homestead.Kitchen");

            Core.Move(new MudObject("glass shards", "These long shards of dirty glass are quite sharp. Be careful."), this, RelativeLocations.CONTENTS);
        }
    }
}