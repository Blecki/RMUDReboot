using RMUD;

namespace World.Wall
{

    public class UpperCatwalk : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Upper Catwalk");
            SetProperty("long", "Up here the air is a bit thin. There's nothing below but fog, and nothing above but the sky. From here, you can just see the edge of the wall - where the gray steel ends and the gray sky begins.");

            OpenLink(Direction.DOWN, "Wall.LowerCatwalk");
            OpenLink(Direction.UP, "Wall.Overhang");
        }
    }
}