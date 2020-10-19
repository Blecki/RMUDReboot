using RMUD;

namespace World
{
    public class Player : RMUD.MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.Actor(this);

            SetProperty("short", "you");
            SetProperty("rank", 500);
        }
    }
}
