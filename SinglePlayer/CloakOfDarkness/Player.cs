using RMUD;

namespace CloakOfDarkness
{
    public class Player : RMUD.MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.Actor(this);

            SetProperty("short", "you");
            Core.Move(Core.GetObject("Cloak"), this, RelativeLocations.WORN);
        }
    }
}
