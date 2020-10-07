using RMUD;

namespace World.Homestead
{

    public class SittingRoom : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Sitting Room");
            SetProperty("long", "The walls of this small sitting room are made of overlapping plates of metal. Many of them still show paint from their previous lives, and have be artfully arranged to evoke a pattern of red and blue stripes. The north wall is stone, rough carved, with a fire pit chiseled sunk into the rock face. The glass in the window on the south wall is shattered. A staircase, almost hidden in the corner, leads downward.");

            OpenLink(Direction.EAST, "Homestead.Kitchen");
            OpenLink(Direction.DOWN, "Homestead.Cellar");

            Move(GetObject("Homestead.Chair"), this, RelativeLocations.Contents);
        }
    }

    public class Chair : RMUD.MudObject
    {
        public Chair()
        {
            Container(RMUD.RelativeLocations.On | RMUD.RelativeLocations.Under, RMUD.RelativeLocations.On);

            Short = "plush chair";
            Long = "Long ago, this was a fine leather chair. These days there are more patches then original construction, though it still appears very plush.";
            GetProperty<NounList>("nouns").Add("plush", "chair");

            Move(GetObject("Homestead.Jaygmundre"), this, RMUD.RelativeLocations.On);

            Check<RMUD.MudObject, RMUD.MudObject>("can take?").Do((actor, thing) =>
            {
                SendMessage(actor, "It's far too heavy.");
                return SharpRuleEngine.CheckResult.Disallow;
            });
        }
    }
}