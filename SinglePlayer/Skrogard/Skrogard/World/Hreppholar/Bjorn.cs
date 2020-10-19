using RMUD;

namespace World.Hreppholar
{
    class Bjorn : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.Actor(this);

            this.Response("who he is", (actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "\"Name's Bjorn,\" <the0> gasps. \"You going to buy something, or..?\"", this);
                    GlobalRules.ConsiderPerformRule("introduce self", this);
                    return PerformResult.Stop;
                });

            Perform<MudObject, MudObject, MudObject>("topic response")
                .When((actor, npc, topic) => topic == null)
                .Do((actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "\"Eh?\" <the0> asks.", this);
                    return PerformResult.Stop;
                });

            Short = "Bjorn";

            AddNoun("shopkeeper", "shop", "keeper", "keep");
            AddNoun("bjorn", "b").When(a => GlobalRules.ConsiderValueRule<bool>("actor knows actor?", a, this));

            this.Wear("overalls", ClothingLayer.Outer, ClothingBodyPart.Legs);
            this.Wear("simple white shirt", ClothingLayer.Outer, ClothingBodyPart.Torso);
            this.Wear("kufi", ClothingLayer.Outer, ClothingBodyPart.Head);

            Perform<MudObject, MudObject>("describe in locale").Do((actor, item) =>
            {
                Core.SendMessage(actor, "The shopkeeper leans on the counter.");
                return PerformResult.Continue;
            });
        }
    }
}