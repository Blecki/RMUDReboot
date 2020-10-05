using RMUD;

namespace World.Surface
{

    public class Hollow : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Hollow");
            SetProperty("long", "There's a small hollow in the junk here, sloping down toward a crack in the metal ground, through which protrudes an excavator bucket.");

            OpenLink(Direction.DOWN, "Caves.ExcavatorCave");
            OpenLink(Direction.EAST, "Surface.WindingPath");
        }
    }
}