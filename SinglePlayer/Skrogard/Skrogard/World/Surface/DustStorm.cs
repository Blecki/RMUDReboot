using RMUD;
using StandardActionsModule;

namespace World.Surface
{

    public class DustStorm : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Dustbowl");
            SetProperty("long", "An endless expanse of dusty plain extends in every direction, and the wind kicks it up into a blinding screen. Glance around once, and you forget from which direction you came.");

            OpenLink(Direction.WEST, "Surface.DustStorm");
            OpenLink(Direction.NORTH, "Surface.DustStorm");
            OpenLink(Direction.EAST, "Surface.DustStorm");
            OpenLink(Direction.SOUTH, "Surface.DustStorm");

            this.PerformGo()
                .Do((Actor, Link) =>
                {
                    if (Random.Next(10) == 0)
                        Link.SetProperty("link destination", "Surface.Dustbowl");
                    else
                        Link.SetProperty("link destination", "Surface.DustStorm");

                    return PerformResult.Continue;
                });
        }
    }
}