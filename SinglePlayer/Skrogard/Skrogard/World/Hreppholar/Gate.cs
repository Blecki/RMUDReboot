using RMUD;

namespace World.Hreppholar
{

    public class Gate : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Gate");
            SetProperty("long", "A gate in the metal wall stands open, hinges rusted. There's a bit of shade underneath it, but it no longer serves any other purpose.");

            OpenLink(Direction.NORTH, "Surface.Dustbowl");
            OpenLink(Direction.SOUTH, "Hreppholar.Square");
        }
    }
}