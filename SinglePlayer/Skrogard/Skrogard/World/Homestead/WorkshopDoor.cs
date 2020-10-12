using RMUD;

namespace World.Homestead
{
    public class WorkshopDoor : RMUD.BasicDoor
    {
        public override void Initialize()
        {
            AddNoun("workshop");
            Short = "workshop door";
            Long = "This metal door has no knob or handle, but appears to swing in both directions.";
        }
    }
}