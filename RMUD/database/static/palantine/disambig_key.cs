public class disambig_key : RMUD.MudObject
{
    public override void Initialize()
    {
        Short = "barrel key";
        GetProperty<NounList>("nouns").Add("KEY", "BARREL");
    }
}