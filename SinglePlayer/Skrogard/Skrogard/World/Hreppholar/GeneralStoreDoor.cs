using RMUD;

namespace World.Hreppholar
{
    public class GeneralStoreDoor : RMUD.BasicDoor
    {
        public override void Initialize()
        {
            AddNoun("general", "store");
            Short = "General Store Door";
            Long = "A simple door of wood and copper mesh.";
        }
    }
}