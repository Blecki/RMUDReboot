using RMUD;

namespace World.Caves
{

    public class ExcavatorCave : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Excavator Cave");
            SetProperty("long", "This small chamber was formed by two massive chunks of metal precariously balanced overhead. An excavator sits right in the center of the space, arm reaching up through the junk.");

            AddScenery("There's no way the excavator will ever work again. It's probably more rust than steel at this point, and the treads have fallen off.", "excavator");

            OpenLink(Direction.WEST, "Caves.JunkChasm");
            OpenLink(Direction.IN, "Caves.ExcavatorCab");
            OpenLink(Direction.UP, "Surface.Hollow");
            OpenLink(Direction.EAST, "Caves.Lake");
        }
    }
}