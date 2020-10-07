using RMUD;

namespace World.Surface
{

    public class Dustbowl : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Dustbowl");
            SetProperty("long", "To the west, a narrow and rocky stream. To the south, the high metal walls of a settlement. Everywhere else - dust. Nothing grows here. Nothing even tries. The ground is perfectly flat. The bottom of an old dried-up lake, perhaps.");

            OpenLink(Direction.WEST, "Surface.Bridge");
            OpenLink(Direction.SOUTH, "Hreppholar.Gate");
            OpenLink(Direction.NORTH, "Surface.DustStorm");
            OpenLink(Direction.EAST, "Surface.DustStorm");
        }
    }
}