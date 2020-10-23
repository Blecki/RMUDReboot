using RMUD;

namespace World.Caves
{
    public class Glade : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Mushroom Glade");
            SetProperty("long", "This chamber is wide and round, and utterly filled by glowing blue mushrooms of all sizes. Small ones, large ones, ones with swirling patterns and ones with stripes.");

            OpenLink(Direction.NORTH, "Caves.Tunnel");
        }
    }
}