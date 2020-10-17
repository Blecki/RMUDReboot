using RMUD;
using StandardActionsModule;

namespace World.Homestead
{
    public class Kitchen : MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Kitchen");
            SetProperty("long", "You are in a small, dirty kitchen. Every surface is covered in some kind of knick-knack, most of them broken.");

            OpenLink(Direction.EAST, "Homestead.Hallway");
            OpenLink(Direction.WEST, "Homestead.SittingRoom");
            OpenLink(Direction.OUT, "Homestead.Homestead");

            Move(GetObject("Homestead.Table"), this);
            Move(GetObject("Homestead.SkullKey"), GetObject("Homestead.Table"), RelativeLocations.ON);
        }
    }

    public class Table : MudObject
    {
        public Table()
        {
            Container(RelativeLocations.ON | RelativeLocations.UNDER, RelativeLocations.ON);

            Short = "metal table";
            Long = "This table is little more than a sheet of metal with some legs grafted on. It is dented and stained and still bares old paint from its days as armor plating.";
            AddNoun("metal", "table");

            Move(new MudObject("empty plasma bag", "A small translucent pouch made of tough plastic. All that's left inside is the residue of a red liquid."), this, RelativeLocations.ON);

            this.CheckCanTake().ThisOnly().Do((actor, thing) =>
            {
                SendMessage(actor, "It's far too heavy.");
                return CheckResult.Disallow;
            });
        }
    }
}