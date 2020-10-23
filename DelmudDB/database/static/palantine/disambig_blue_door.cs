using RMUD;
using static RMUD.Core;

public class disambig_blue_door : MudObject
{
    public override void Initialize()
    {
        AddNoun("DOOR");

        // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
        AddNoun("CLOSED").When(actor => !GetProperty<bool>("open?"));
        AddNoun("OPEN").When(actor => GetProperty<bool>("open?"));

        SetProperty("open?", false);
        SetProperty("openable?", true);

        SetProperty("lockable?", true);

        AddNoun("BLUE");

        this.CheckIsMatchingKey().Do((door, key) =>
        {
            if (object.ReferenceEquals(key, Core.GetObject("palantine\\library_key")))
                return CheckResult.Allow;
            return CheckResult.Disallow;
        });

        Short = "blue door";
    }
}
