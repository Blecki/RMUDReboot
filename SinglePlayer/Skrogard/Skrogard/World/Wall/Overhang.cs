using RMUD;

namespace World.Wall
{

    public class Overhang : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Overhang");
            SetProperty("long", "A metal platform juts out from the top of the wall, a ladder piercing the floor. Only a thin railing stands between you and a long fall into the junk.");

            OpenLink(Direction.DOWN, "Wall.UpperCatwalk");
            OpenLink(Direction.WEST, "Wall.Perch");
        }
    }
}