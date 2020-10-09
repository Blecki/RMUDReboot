using RMUD;

namespace World.Wilderness
{
    public class Clearing : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Clearing");
            SetProperty("long", "Several paths cross, forming a triangle with a patch of grass in the middle. Several pink fungi grow in a circle around the path of grass.");

            OpenLink(Direction.SOUTHWEST, "Wilderness.TopOfTheLog");
            OpenLink(Direction.WEST, "Wilderness.Slope");
            OpenLink(Direction.EAST, "Wilderness.FlowerTrail");
        }
    }
}