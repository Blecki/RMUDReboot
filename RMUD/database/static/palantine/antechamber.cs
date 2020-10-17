public class antechamber : MudObject
{
    public bool SecretPassageOpen = false;

	public override void Initialize()
	{
        Room(RoomType.Interior);
        SetProperty("ambient light", LightingLevel.Bright);

        Short = "Palantine Villa - Antechamber";
        Long = "Two imposing statues stand guard in this small room, on either side of the door to the room beyond. On the left, Jupiter, king of the gods. On the right, Minerva, the goddess of wisdom and beauty.";

        AddScenery(new Jupiter());
        AddScenery("Minerva is turned to regard her father Jupiter, and poses with one hand on her hips and the other on the shaft of a massive hammer.", "minerva");
        var hammer = AddScenery("It's really quite impressive. Despite the hammer's massive size, Minerva's grip is rather dainty. You could pull the hammer right out.", "hammer", "shaft");

        hammer.Check<MudObject, MudObject>("can pull?").Do((actor, thing) => CheckResult.Allow);
        hammer.Perform<MudObject, MudObject>("pull")
            .Do((actor, thing) =>
            {
                if (SecretPassageOpen)
                    SendMessage(actor, "The hammer doesn't budge.");
                else
                {
                    SecretPassageOpen = true;
                    OpenLink(Direction.SOUTH, "palantine/pit");
                    SendMessage(actor, "The hammer slides up a few inches, then lodges firmly in place. With a rumble, a section of the south wall slides aside.");
                }
                return PerformResult.Stop;
            });

        var table = new Table();
        Move(table, this);

        Move(new RMUD.MudObject("old vase", "An old, cracked vase."), table);
        Move(GetObject("palantine/ball"), this);


        OpenLink(RMUD.Direction.NORTH, "palantine/disambig");
        OpenLink(RMUD.Direction.EAST, "palantine/solar");
        OpenLink(RMUD.Direction.WEST, "palantine/garden");
    }
}

public class Jupiter : RMUD.MudObject
{
    public Jupiter()
    {
        AddNoun("jupiter");
        Long = "Jupiter holds in his left hand a gleaming thunderbolt. It glows bright enough to light the entire chamber. In his right, he holds a chisel.";

        Value<RMUD.MudObject, RMUD.LightingLevel>("light level").Do(a => RMUD.LightingLevel.Bright);

        SetProperty("scenery?", true); // We could also add a 'should be listed in locale?' rule to Jupiter.
    }
}

public class Table : RMUD.MudObject
{
    public Table()
    {
        Container(RMUD.RelativeLocations.ON | RMUD.RelativeLocations.UNDER, RMUD.RelativeLocations.ON);

        Short = "ancient table";
        Long = "As the years have worn long the wood of this table has dried and shrunk, and split, and what was once a finely crafted table is now pitted and gouged. The top is still mostly smooth, from use but not from care.";
        AddNoun("ancient", "table");
        SetProperty("article", "an");

        Move(new RMUD.MudObject("matchbook", "A small book of matches with a thunderbolt on the cover."), this, RMUD.RelativeLocations.UNDER);

        this.CheckCanTake().Do((actor, thing) =>
        {
            SendMessage(actor, "It's far too heavy.");
            return CheckResult.Disallow;
        });

        this.CheckCanPushDirection().Do((actor, subject, link) => CheckResult.Allow);

        //Value<RMUD.MudObject, RMUD.MudObject, string, string>("printed name").When((viewer, thing, article) => thing == this).Do((viewer, thing, article) => "an ancient table");
    }
}
