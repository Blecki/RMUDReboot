using RMUD;

namespace World.Hreppholar
{
    public class EastStreet : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Dusty Street");
            SetProperty("long", "You are on a dusty street between ramshackle houses made of old sheet metal. Stunted trees stand at the corners of the buildings.");

            OpenLink(Direction.WEST, "Hreppholar.Square");
            OpenLink(Direction.NORTH, "Hreppholar.BlueHouse");
            OpenLink(Direction.SOUTH, "Hreppholar.RedHouse");
        }
    }
}