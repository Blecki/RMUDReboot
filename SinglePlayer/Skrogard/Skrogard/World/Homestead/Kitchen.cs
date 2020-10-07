using RMUD;

namespace World.Homestead
{

    public class Kitchen : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Kitchen");
            SetProperty("long", "You are in a small, dirty kitchen. Every surface is covered in some kind of knick-knack, most of them broken.");

            OpenLink(Direction.EAST, "Homestead.Hallway");
            OpenLink(Direction.WEST, "Homestead.SittingRoom");
            OpenLink(Direction.OUT, "Homestead.Homestead");

            Move(GetObject("Homestead.Table"), this, RelativeLocations.Contents);
            Move(GetObject("Homestead.SkullKey"), GetObject("Homestead.Table"), RelativeLocations.On);
        }
    }

    public class Table : RMUD.MudObject
    {
        public Table()
        {
            Container(RMUD.RelativeLocations.On | RMUD.RelativeLocations.Under, RMUD.RelativeLocations.On);

            Short = "metal table";
            Long = "This table is little more than a sheet of metal with some legs grafted on. It is dented and stained and still bares old paint from its days as armor plating.";
            GetProperty<NounList>("nouns").Add("metal", "table");

            Move(new RMUD.MudObject("empty plasma bag", "A small translucent pouch made of tough plastic. All that's left inside is the residue of a red liquid."), this, RMUD.RelativeLocations.On);

            Check<RMUD.MudObject, RMUD.MudObject>("can take?").ThisOnly().Do((actor, thing) =>
            {
                SendMessage(actor, "It's far too heavy.");
                return SharpRuleEngine.CheckResult.Disallow;
            });
        }
    }
}