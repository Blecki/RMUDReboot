using RMUD;

namespace World.Homestead
{

    public class Ledge : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Ledge");
            SetProperty("long", "A narrow ledge, carved with steps, runs around the rock from the homestead's front door on the shelf above to the bottom of the gully behind the stone.");

            OpenLink(Direction.WEST, "Homestead.Homestead");
            OpenLink(Direction.NORTH, "Homestead.Gully");

        }
    }
}