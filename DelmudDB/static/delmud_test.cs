public class delmud_test : MudObject
{
    public override void Initialize()
    {
        ObjectDecorator.LockedDoor(this);

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
