using RMUD;

namespace World.Caves
{

    public class Pod : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Interior);

            SetProperty("short", "$Red;Pod$White;");
            SetProperty("long", "You are in a small dark pod. Light falls through cracks above. The metal walls glisten with condensation, and some papers crinkle under foot. A single light glares overhead.");
            SetProperty("ambient light", LightingLevel.Bright);

            Core.Move(GetObject("Caves.Papers"), this);
            Core.Move(GetObject("Caves.Pipe"), this);
            Core.Move(GetObject("Caves.Target"), this);

            AddScenery("The papers appear to be blank, save for one.", "papers");

            OpenLink(Direction.OUT, "Caves.DeepJunk");

            PropertyModifier<int>("combat_hit_modifier").Do((o, v) => { v.Value += 4; return PerformResult.Continue; })
                .Name("Extra hit in pod rule");
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

    public class Pipe: MudObject
    {
        public override void Initialize()
        {
            base.Initialize();

            SimpleName("pipe");
            Long = "A sturdy length of pipe.";
            SetProperty("combat_damage_die", "2d6");

            PropertyModifier<int>("combat_hit_modifier").Do((o, v) => { v.Value += 6; return PerformResult.Continue; });
            PropertyModifier<string>("combat_hit_modifier").Do((o, v) => { v.Value = "error?"; return PerformResult.Stop; });

        }
    }

    public class Target: MudObject
    {
        public override void Initialize()
        {
            base.Initialize();

            SimpleName("target");
            SetProperty("combat_health", 10);


    }
}
}