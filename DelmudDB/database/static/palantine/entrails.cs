using RMUD;
using static RMUD.Core;

class entrails : MudObject
{
    public override void Initialize()
    {
        Short = "entrails";
        AddNoun("entrails");

        SetProperty("clothing layer", ClothingLayer.Over);
        SetProperty("clothing part", ClothingBodyPart.Cloak);
        SetProperty("wearable?", true);
        SetProperty("article", "some");

        Perform<MudObject, MudObject>("drop").Do((actor, item) =>
            {
                var wolf = GetObject("palantine/wolf");
                if (wolf.Location.HasValue(out var wolfLoc) && actor.Location.HasValue(out var pLoc) && wolfLoc == pLoc)
                {
                    ConsiderPerformRule("handle-entrail-drop", wolf, this);
                    return PerformResult.Stop;
                }
                return PerformResult.Continue;
            });
    }
}