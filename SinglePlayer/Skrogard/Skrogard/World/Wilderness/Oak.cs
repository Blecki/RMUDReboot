using RMUD;

namespace World.Wilderness
{
    public class Oak : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Oak");
            SetProperty("long", "A massive oak stands in your way. The trail splits around it.");

            OpenLink(Direction.NORTHWEST, "Wilderness.FlowerTrail");

            Core.Move(GetObject("Wilderness.MotherTree"), this);
        }
    }
}