using RMUD;

namespace World.Homestead
{
    public class WorkshopDoor : RMUD.BasicDoor
    {
        public override void Initialize()
        {
            GetProperty<NounList>("nouns").Add("workshop");
            Short = "workshop door";
            Long = "This metal door has no knob or handle, but appears to swing in both directions.";
        }
    }
}