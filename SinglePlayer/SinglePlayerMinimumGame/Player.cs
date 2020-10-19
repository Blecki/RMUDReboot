using RMUD;

namespace Minimum
{
    public class Player : RMUD.MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.Actor(this);

            SetProperty("short", "you");
        }
    }
}
