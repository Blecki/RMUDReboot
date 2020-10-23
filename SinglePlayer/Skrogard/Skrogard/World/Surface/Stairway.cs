using RMUD;

namespace World.Surface
{

    public class Stairway : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Stairway");
            SetProperty("long", "This stairway carved into the stone, then into the junk, climbs ever higher between two walls of metal. The stairs have been welded together so that they do not shift during the junk quakes.");

            OpenLink(Direction.SOUTH, "Surface.Overlook");
            OpenLink(Direction.WEST, "Surface.Landing");
        }
    }
}