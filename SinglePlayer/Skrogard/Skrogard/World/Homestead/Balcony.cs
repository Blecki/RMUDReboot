using RMUD;

namespace World.Homestead
{

    public class Balcony : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Balcony");
            SetProperty("long", "A rickety wooden platform juts from the side of the rock. It creaks loudly underfoot.");

            OpenLink(Direction.DOWN, "Homestead.Gully");
            OpenLink(Direction.WEST, "Homestead.Cellar", Core.GetObject("Homestead.StormDoor@outside"));

        }
    }
}