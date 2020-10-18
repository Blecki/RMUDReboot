using RMUD;

namespace CloakOfDarkness
{
    public class Player : RMUD.MudObject
    {
        public override void Initialize()
        {
            Actor();

            SetProperty("short", "you");
            Core.Move(GetObject("Cloak"), this, RelativeLocations.WORN);
        }
    }
}
