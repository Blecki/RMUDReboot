public class disambig_blue_door : RMUD.LockedDoor
{
    public override void Initialize()
    {
        AddNoun("BLUE");
        Locked = true;
        IsMatchingKey = k => object.ReferenceEquals(k, GetObject("palantine\\library_key"));
        Short = "blue door";
    }
}
