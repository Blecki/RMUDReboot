using RMUD;
using QuestModule;

namespace World.Homestead
{
    class PlasmaQuest : MudObject
    {
        public override void Initialize()
        {
            Short = "Get Plasma for Jaygmundre";

            bool Active = false;

            Value<MudObject, MudObject, bool>("quest available?").Do((actor, quest) => !Active && IsVisibleTo(actor, MudObject.GetObject("Homestead.Jaygmundre")));

            Value<MudObject, MudObject, bool>("quest complete?").Do((actor, quest) =>
            {
                return ConsiderValueRule<bool>("plasma-quest-complete", GetObject("Homestead.Jaygmundre"));
            });

            //Value<MudObject, MudObject, bool>("quest failed?").Do((actor, quest) => !ObjectContainsObject(actor, GetObject("palantine/entrails")));

            Perform<MudObject, MudObject>("quest accepted").Do((questor, quest) =>
                {
                    Active = true;
                    SendMessage(questor, "You have accepted the quest: Get Plasma for Jaygmundre.");
                    SendMessage(questor, "\"Really? Thanks kid. I'll be waiting here,\" <the0> says.", GetObject("Homestead.Jaygmundre"));
                    return PerformResult.Continue;
                });

            Perform<MudObject, MudObject>("quest completed").Do((questor, quest) =>
                {
                    SendMessage(questor, "Get Plasma for Jaygmundre: completed.");
                    this.ResetQuestObject(GetObject("Homestead.Jaygmundre"));
                    Active = false;
                    return PerformResult.Continue;
                });
        }
    }
}