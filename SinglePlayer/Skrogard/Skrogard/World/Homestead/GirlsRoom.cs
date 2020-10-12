using RMUD;

namespace World.Homestead
{

    public class GirlsRoom : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Girl's Bed Room");
            SetProperty("long", "Every surface in this tiny bedroom is scribed with swirling leaves and bursting flowers, and a great deal of dust. Save for the dust, the floor is immaculate. The bed is neatly made, the dresser clear. A few wilted stuffed animals sit on the bed.");

            OpenLink(Direction.NORTH, "Homestead.Hallway", GetObject("Homestead.FlowerDoor@inside"));

            Move(GetObject("Homestead.Bed"), this);
        }
    }

    public class Bed : RMUD.MudObject
    {
        public Bed()
        {
            Container(RMUD.RelativeLocations.On | RMUD.RelativeLocations.Under, RMUD.RelativeLocations.On);

            Short = "small bed";
            Long = "This small, plush bed looks quite soft.";
            AddNoun("small", "bed");

            Check<RMUD.MudObject, RMUD.MudObject>("can take?")
                .ThisOnly()
                .Do((actor, thing) =>
            {
                SendMessage(actor, "It's far too heavy.");
                return SharpRuleEngine.CheckResult.Disallow;
            });

            Move(GetObject("Homestead.Skull"), this, RelativeLocations.On);
        }
    }
}