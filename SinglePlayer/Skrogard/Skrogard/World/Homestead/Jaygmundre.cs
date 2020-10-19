using RMUD;

namespace World.Homestead
{
    class Jaygmundre : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.Actor(this);

            this.Response("who he is", (actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "\"Jaygmunder,\" <the0> gasps. \"You can call me Jay though.\"", this);
                    GlobalRules.ConsiderPerformRule("introduce self", this);
                    return PerformResult.Stop;
                });

            this.Response("his mechanical suit", "\"Huh, my suit? Whhat about it? When you get old, you'll need some help getting around too.\" <the0> sighs. \"Not that it does me much good now. I have't got any plasma for it.\"");

            this.Response("his plush chair", "\"Yeah it's pretty nice.\" <the0> pauses to stroke the arm of his chair. \"I'd let you sit in it if I could get up. Need plasma for that, though.\"");

            this.Response("plasma", (actor, nps, topic) =>
            {
                Core.SendLocaleMessage(actor, "\"The red stuff? In the bags? You know what plasma is, don't you?\"");
                var quest = Core.GetObject("Homestead.PlasmaQuest");
                if (GlobalRules.ConsiderValueRule<bool>("quest available?", actor, quest))
                {
                    Core.SendMessage(actor, "\"Think you can grab some for me?\" <the0> asks.", this);
                    this.OfferQuest(actor, quest);
                }
                return PerformResult.Stop;
            });

            Perform<MudObject, MudObject, MudObject>("topic response")
                .When((actor, npc, topic) => topic == null)
                .Do((actor, npc, topic) =>
                {
                    Core.SendLocaleMessage(actor, "\"Wut?\" <the0> asks.", this);
                    return PerformResult.Stop;
                });

            Short = "Jaygmundre";

            AddNoun("jaygmundre", "jay", "j").When(a => GlobalRules.ConsiderValueRule<bool>("actor knows actor?", a, this));

            this.Wear("mechanical suit", ClothingLayer.Outer, ClothingBodyPart.Torso);
        }
    }
}