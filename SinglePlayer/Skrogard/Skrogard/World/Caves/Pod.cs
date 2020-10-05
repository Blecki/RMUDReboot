using RMUD;

namespace World.Caves
{

    public class Pod : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Interior);

            SetProperty("short", "Pod");
            SetProperty("long", "You are in a small dark pod. Light falls through cracks above. The metal walls glisten with condensation, and some papers crinkle under foot. A single light glares overhead.");
            SetProperty("ambient light", LightingLevel.Bright);

            Move(GetObject("Papers"), this, RelativeLocations.Contents);

            AddScenery("The papers appear to be blank, save for one.", "papers");

            OpenLink(Direction.OUT, "Caves.DeepJunk");
        }
    }

    public class Papers : RMUD.MudObject
    {
        public override void Initialize()
        {
            base.Initialize();

            SimpleName("paper");
            Long = "This seems to be a manifest of some kind. It's a bit smudged, but it seems to be an order for 1 junker.";
        }
    }
}