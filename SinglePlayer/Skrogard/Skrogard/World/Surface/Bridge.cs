using RMUD;

namespace World.Surface
{

    public class Bridge : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Causeway");
            SetProperty("long", "A wooden causeway extends across the stream, east to west. There are no railings, but the drop is also only a few feet to a stream that barely counts as damp.");

            OpenLink(Direction.WEST, "Surface.LowerBadlands");
            OpenLink(Direction.EAST, "Surface.Dustbowl");
        }
    }
}