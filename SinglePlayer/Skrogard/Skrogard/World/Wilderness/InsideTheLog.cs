using RMUD;

namespace World.Wilderness
{
    public class InsideTheLog : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Inside the Log");
            SetProperty("long", "The path passes through the interior of a massive log. Above, you can see another path, but there's no way to climb up because of an abundance of thorns.");

            OpenLink(Direction.SOUTHEAST, "Wilderness.LowerRavine");
            OpenLink(Direction.NORTHWEST, "Wilderness.Ravine");
        }
    }
}