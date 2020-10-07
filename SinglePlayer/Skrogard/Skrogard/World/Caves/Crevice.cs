using RMUD;

namespace World.Caves
{

    public class Chasm : RMUD.MudObject
    {
        public override void Initialize()
        {
            Room(RoomType.Exterior);

            SetProperty("short", "Chasm");
            SetProperty("long", "You are the bottom of a vast chasm in the junk. The bottom is dotted with deep gouges where the junk has clawed at the rock, and water forms acrid pools in them. Small fungi, emitting a blue glow, grow in rows along the gouges.");

            OpenLink(Direction.SOUTH, "Caves.Tunnel");
            OpenLink(Direction.EAST, "Caves.Crevice");
        }
    }
}