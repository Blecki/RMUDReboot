using RMUD;

namespace World.Hreppholar
{
    public class GeneralStore : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "General Store");
            SetProperty("long", "Dusty shelves stand in neat rows, filled with goods of all kinds. New mixed with old - clean pieces of glass ware and canned goods, old iron pots, clothes both tattered and new hanging on long rods. A counter stands at the back.");

            OpenLink(Direction.NORTH, "Hreppholar.GeneralStorePorch", GetObject("Hreppholar.GeneralStoreDoor@inside"));
            OpenLink(Direction.SOUTH, "Hreppholar.GeneralStoreBackRoom");

            Core.Move(GetObject("Hreppholar.Bjorn"), this);
        }
    }
}