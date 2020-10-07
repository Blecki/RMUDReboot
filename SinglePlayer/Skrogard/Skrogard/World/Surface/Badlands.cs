using RMUD;

namespace World.Surface
{

    public class Badlands : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Badlands");
            SetProperty("long", "A hard tack path winds through steep hills, large rocks rising on both sides.");

            OpenLink(Direction.NORTH, "Homestead.Homestead");
            OpenLink(Direction.SOUTH, "Surface.LowerBadlands");
        }
    }
}