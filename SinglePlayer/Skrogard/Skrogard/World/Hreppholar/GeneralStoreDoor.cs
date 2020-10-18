using RMUD;

namespace World.Hreppholar
{
    public class GeneralStoreDoor : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.BasicDoor(this);

            AddNoun("general", "store");
            Short = "General Store Door";
            Long = "A simple door of wood and copper mesh.";
        }
    }
}