using RMUD;

namespace World.Surface
{

    public class Landing : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Landing");
            SetProperty("long", "You are on a small landing at the top of the metal stairway, right against the base of the wall. The wall extends upwards as far as you can see. A ladder leads upwards.");

            OpenLink(Direction.EAST, "Surface.Stairway");
            OpenLink(Direction.UP, "Wall.LowerCatwalk");
        }
    }
}