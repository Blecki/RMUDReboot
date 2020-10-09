using RMUD;

namespace World.Hreppholar
{

    public class Square : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Square");
            SetProperty("long", "You are in a dusty square, surprisingly large compared to the size of the settlement. This is the only place in the village with pavement, though it's as much crack now as asphalt. There's no vehicles on the roads, just weeds and bits of debris. A dried up fountain stands in the very center.");

            OpenLink(Direction.NORTH, "Hreppholar.Gate");
            OpenLink(Direction.EAST, "Hreppholar.EastStreet");
            OpenLink(Direction.WEST, "Hreppholar.WestStreet");
            OpenLink(Direction.SOUTH, "Hreppholar.GeneralStorePorch");

            Move(GetObject("Hreppholar.Fountain"), this);
        }
    }

    public class Fountain : RMUD.MudObject
    {
        public Fountain()
        {
            Container(RMUD.RelativeLocations.In, RMUD.RelativeLocations.In);

            Short = "fountain";
            Long = "This old fountain is made from welded together bits of scrap. It depicts a girl, lips pursed, who would be spurting a stream of water if the pump still worked.";
            GetProperty<NounList>("nouns").Add("old", "dried", "fountain", "dried-up");

            Move(new RMUD.MudObject("steel penny", "A small coin marked on one side with a serpent, and the other with a woman's face."), this, RMUD.RelativeLocations.In);

            Check<RMUD.MudObject, RMUD.MudObject>("can take?").ThisOnly().Do((actor, thing) =>
            {
                SendMessage(actor, "That appears to be attached rather securely to the ground.");
                return SharpRuleEngine.CheckResult.Disallow;
            });
        }
    }
}