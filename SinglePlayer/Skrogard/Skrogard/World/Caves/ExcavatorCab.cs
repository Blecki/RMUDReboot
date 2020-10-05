using RMUD;

namespace World.Caves
{

    public class ExcavatorCab : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Excavator Cab");
            SetProperty("long", "You are sitting in the cab of an old, broken down excavator.");

            OpenLink(Direction.OUT, "Caves.ExcavatorCave");
        }
    }
}