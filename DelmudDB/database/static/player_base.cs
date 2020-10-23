using RMUD;
using static RMUD.Core;

public class player_base : MudObject
{
    public override void Initialize()
    {
        Actor();

        SetProperty("combat health", 10);
    }
}

