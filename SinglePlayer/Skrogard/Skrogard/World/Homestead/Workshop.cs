using RMUD;

namespace World.Homestead
{

    public class Workshop : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Workshop");
            SetProperty("long", "Tables piled high with junk sit against all three walls of this small shelter. The minimal roof and open sides have done little to protect the contents from the elements, and everything is covered in a thick patina of rust.");

            OpenLink(Direction.OUT, "Homestead.Gully");
            OpenLink(Direction.WEST, "Caves.Passage", Core.GetObject("Homestead.WorkshopDoor@outside"));

        }
    }

    // Todo: Taking of junk, modelled after the library of kuz
}