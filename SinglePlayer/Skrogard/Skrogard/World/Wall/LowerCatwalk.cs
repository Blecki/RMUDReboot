using RMUD;

namespace World.Wall
{

    public class LowerCatwalk : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Lower Catwalk");
            SetProperty("long", "A narrow metal ledge is grafted to the wall. It offers a nice place for anyone making the arduous climb up or down to rest.");

            OpenLink(Direction.DOWN, "Surface.Landing");
            OpenLink(Direction.UP, "Wall.UpperCatwalk");
        }
    }
}