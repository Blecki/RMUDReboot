using RMUD;

namespace World.Surface
{

    public class Overlook : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Overlook");
            SetProperty("long", "On the edge of the junk field, you have a great view. You are standing on a large boulder. In the valley below, several thin columns of smoke rise into the still air. Above to the west, a massive gray barrier rises against the sky. A wall of metal, so high the top edge is lost in the haze.");

            OpenLink(Direction.WEST, "Surface.WindingPath");
            OpenLink(Direction.NORTH, "Surface.Stairway");
            OpenLink(Direction.SOUTH, "Homestead.Homestead");
        }
    }
}