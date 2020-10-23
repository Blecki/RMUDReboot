using RMUD;

namespace World.Wilderness
{
    public class TopOfTheLog : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "On Top of a Giant Log");
            SetProperty("long", "The path crosses a giant log. Below, you can see another path, but thick and wicked looking thorn bushes block you from reaching it.");

            OpenLink(Direction.NORTHEAST, "Wilderness.Clearing");
            OpenLink(Direction.SOUTHWEST, "Wilderness.Trail");
        }
    }
}