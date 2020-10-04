public class library_key : RMUD.MudObject
{
    public override void Initialize()
    {
        Short = "spade key";
        GetProperty<NounList>("nouns").Add("KEY", "SPADE");
    }
}