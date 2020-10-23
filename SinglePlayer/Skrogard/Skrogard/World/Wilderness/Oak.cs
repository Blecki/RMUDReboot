using RMUD;

namespace World.Wilderness
{
    public class Oak : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Oak");
            SetProperty("long", "A massive oak stands in your way. The trail splits around it.");

            OpenLink(Direction.NORTHWEST, "Wilderness.FlowerTrail");

            Core.Move(Core.GetObject("Wilderness.MotherTree"), this);
        }
    }
}