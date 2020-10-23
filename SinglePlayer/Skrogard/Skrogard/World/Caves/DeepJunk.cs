using RMUD;

namespace World.Caves
{

    public class DeepJunk : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Deep Junk");
            SetProperty("long", "You are in a cave made of metal. Piles of trash loom all around you; old decayed machinery and bits of blasted armor form the floor and walls. Shapes jump out of the darkness. The only light, filtered through dust and cracks high above, casts deep shadows. A dented pod sits in the center of the cave.");

            AddScenery("The pod is beat up and a bit rusty. It looks very old.", "pod");

            OpenLink(Direction.IN, "Caves.Pod");
            OpenLink(Direction.EAST, "Caves.JunkChasm");
        }
    }
}