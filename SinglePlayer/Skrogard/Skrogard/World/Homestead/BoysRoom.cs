using RMUD;

namespace World.Homestead
{

    public class BoysRoom : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Boy's Bed Room");
            SetProperty("long", "An odor lingers in the air about this room. The bed is a thin piece of foam on metal slats, no spread. Piles of moulding clothes sit in the corners. A thick layer of dust clings to the floor, now marred only by your footprints.");

            OpenLink(Direction.SOUTH, "Homestead.Hallway", Core.GetObject("Homestead.PosterDoor@inside"));
        }
    }
}