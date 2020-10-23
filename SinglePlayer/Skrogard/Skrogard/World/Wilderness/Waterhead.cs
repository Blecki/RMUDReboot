using RMUD;

namespace World.Wilderness
{
    public class Waterhead : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Waterhead");
            SetProperty("long", "You are on a narrow path between twisted trees and rocks, near the top of a narrow waterfall.");

            OpenLink(Direction.SOUTHWEST, "Homestead.Gully");
            OpenLink(Direction.EAST, "Wilderness.Trail");
        }
    }
}