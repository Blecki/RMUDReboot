using RMUD;

namespace World.Wilderness
{
    public class Ravine : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Ravine");
            SetProperty("long", "You are at the bottom of a ravine between two walls of muddy stone. Roots stream down over the edges above, dripping with water.");

            OpenLink(Direction.EAST, "Wilderness.Slope");
            OpenLink(Direction.SOUTHEAST, "Wilderness.InsideTheLog");
        }
    }
}