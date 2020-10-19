using RMUD;

namespace World.Homestead
{
    public class PosterDoor : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.LockedDoor(this);

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