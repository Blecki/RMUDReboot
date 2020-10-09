using RMUD;

namespace World.Hreppholar
{
    public class GeneralStorePorch : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Porch of the General Store");
            SetProperty("long", "The porch of the general store is - like, well, almost everything else in this blighted land - made of rusted and patched old metal sheets. It clangs loudly underfoot.");

            OpenLink(Direction.NORTH, "Hreppholar.Square");
            OpenLink(Direction.EAST, "Hreppholar.BackAlley");
            OpenLink(Direction.SOUTH, "Hreppholar.GeneralStore", GetObject("Hreppholar.GeneralStoreDoor@outside"));
        }
    }
}