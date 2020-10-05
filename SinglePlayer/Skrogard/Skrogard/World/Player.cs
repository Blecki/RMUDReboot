using RMUD;

namespace World
{
    public class Player : RMUD.MudObject
    {
        public override void Initialize()
        {
            Actor();

            SetProperty("short", "you");
            SetProperty("rank", 500);
        }
    }
}
