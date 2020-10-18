using RMUD;

namespace World.Homestead
{
    public class WorkshopDoor : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.BasicDoor(this);

            AddNoun("workshop");
            Short = "workshop door";
            Long = "This metal door has no knob or handle, but appears to swing in both directions.";
        }
    }
}