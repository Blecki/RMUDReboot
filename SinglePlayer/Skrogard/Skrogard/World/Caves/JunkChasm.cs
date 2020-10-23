using RMUD;

namespace World.Caves
{

    public class JunkChasm : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Junk Chasm");
            SetProperty("long", "Here the cave opens up on one side into a deep chasm, the bottom lost in darkness. It's impossible to tell how far away the other side is.");

            OpenLink(Direction.WEST, "Caves.DeepJunk");
            OpenLink(Direction.EAST, "Caves.ExcavatorCave");
        }
    }
}