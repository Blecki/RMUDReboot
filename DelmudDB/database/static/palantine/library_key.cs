using RMUD;
using static RMUD.Core;

public class library_key : RMUD.MudObject
{
    public override void Initialize()
    {
        Short = "spade key";
        AddNoun("KEY", "SPADE");
    }
}