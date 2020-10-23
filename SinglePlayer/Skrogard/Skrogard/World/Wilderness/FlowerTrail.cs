using RMUD;

namespace World.Wilderness
{
    public class FlowerTrail : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Flower Trail");
            SetProperty("long", "A path trails through the forest, flowers dotting the edges on both sides.");

            OpenLink(Direction.WEST, "Wilderness.Clearing");
            OpenLink(Direction.SOUTHEAST, "Wilderness.Oak");
        }
    }
}