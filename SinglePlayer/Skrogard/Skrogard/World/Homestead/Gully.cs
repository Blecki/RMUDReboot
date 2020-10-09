using RMUD;

namespace World.Homestead
{

    public class Gully : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Bottom of the Gully");
            SetProperty("long", "A stream runs from the shadow of a massive boulder and across a short stone ledge before hurling itself over the side in a tall, sparkling waterfall. Built under the rock is some kind of shelter with a corrugated metal roof and open sides. Above that, a rickety balcony protrudes from the side of the rock, reachable by a crudely built ladder.");

            OpenLink(Direction.IN, "Homestead.Workshop");
            OpenLink(Direction.SOUTH, "Homestead.Ledge");
            OpenLink(Direction.UP, "Homestead.Balcony");
            OpenLink(Direction.NORTHEAST, "Wilderness.Waterhead");
        }
    }
}