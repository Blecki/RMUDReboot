using RMUD;

namespace World.Wilderness
{
    public class Trail : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Narrow Trail");
            SetProperty("long", "A narrow path winds between stunted and twisted trees. Somewhere above, a waterfall crashes down, filling the air with a fine mist. Near the bottom of this path, the foilage thickens, and the trees turn lush.");

            OpenLink(Direction.NORTHEAST, "Wilderness.TopOfTheLog");
            OpenLink(Direction.WEST, "Wilderness.Waterhead");
        }
    }
}