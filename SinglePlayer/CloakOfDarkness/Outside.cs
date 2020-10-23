using RMUD;

namespace CloakOfDarkness
{
    public class Outside : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Outside the Opera House");
        }
    }
}