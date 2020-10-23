using RMUD;

namespace World.Homestead
{
    public class PosterDoor : MudObject
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

            AddNoun("poster");

            this.CheckIsMatchingKey().Do((door, key) =>
            {
                if (object.ReferenceEquals(key, Core.GetObject("Homestead.SkullKey")))
                    return CheckResult.Allow;
                return CheckResult.Disallow;
            });

            Short = "poster door";
            Long = "This metal door has a poster of a blood mech on it. The blood mech has a skull for a head.";
        }
    }
}