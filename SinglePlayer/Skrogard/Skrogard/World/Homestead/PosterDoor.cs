using RMUD;

namespace World.Homestead
{
    public class PosterDoor : RMUD.LockedDoor
    {
        public override void Initialize()
        {
            GetProperty<NounList>("nouns").Add("poster");
            Locked = true;
            IsMatchingKey = k => object.ReferenceEquals(k, GetObject("Homestead.SkullKey"));
            Short = "poster door";
            Long = "This metal door has a poster of a blood mech on it. The blood mech has a skull for a head.";
        }
    }
}