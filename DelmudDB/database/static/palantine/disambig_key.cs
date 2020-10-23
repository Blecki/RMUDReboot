using RMUD;
using static RMUD.Core;

public class disambig_key : RMUD.MudObject
{
    public override void Initialize()
    {
        Short = "barrel key";
        AddNoun("KEY", "BARREL");
    }
}