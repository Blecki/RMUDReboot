using RMUD;

namespace World.Wilderness
{
    class MotherTree : MudObject
    {
        public override void Initialize()
        {
            Actor();

            this.SetProperty("gender", Gender.Female);
            Long = "The most striking feature of this tree is not it's size - it's the face hacked crudely into the trunk. As you watch, it shifts, as if muttering to itself.";

            this.Response("who she is", (actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "The breeze whistles between the trees. It almost sounds like words.... \"...Forest...\", the wind says.", this);
                    GlobalRules.ConsiderPerformRule("introduce self", this);
                    return PerformResult.Stop;
                });

            Perform<MudObject, MudObject, MudObject>("topic response")
                .When((actor, npc, topic) => topic == null)
                .Do((actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "The wind whistles.", this);
                    return PerformResult.Stop;
                });

            Short = "Mother Tree";

            AddNoun("mother", "tree", "oak", "forest");

            Perform<MudObject, MudObject>("describe in locale").Do((actor, item) =>
            {
                Core.SendMessage(actor, "The wind whistles through the leaves.");
                return PerformResult.Continue;
            });
        }
    }
}