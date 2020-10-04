public class disambig_blue_door : RMUD.LockedDoor
{
    public override void Initialize()
    {
        GetProperty<NounList>("nouns").Add("BLUE");
        Locked = true;
        IsMatchingKey = k => object.ReferenceEquals(k, GetObject("palantine\\library_key"));
        Short = "blue door";
    }
}
