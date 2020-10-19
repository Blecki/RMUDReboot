public class disambig_red_door : MudObject
{
    public override void Initialize()
    {
        ObjectDecorator.LockedDoor(this);

        AddNoun("RED");

        this.CheckIsMatchingKey().Do((door, key) =>
        {
            if (object.ReferenceEquals(key, Core.GetObject("palantine\\disambig_key")))
                return CheckResult.Allow;
            return CheckResult.Disallow;
        });

        Short = "red door";
    }
}
