using RMUD;

namespace World.Surface
{

    public class WindingPath : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Winding Path");
            SetProperty("long", "A winding path snakes through the junk east to west. You can't see very far ahead because the metal blocks your view, but you also can't really get lost.");

            OpenLink(Direction.WEST, "Surface.Hollow");
            OpenLink(Direction.EAST, "Surface.Overlook");
        }
    }
}