using RMUD;

namespace World.Homestead
{
    public class WorkshopDoor : MudObject
    {
        public override void Initialize()
        {
            AddNoun("DOOR");

            // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
            AddNoun("CLOSED").When(actor => !GetProperty<bool>("open?"));
            AddNoun("OPEN").When(actor => GetProperty<bool>("open?"));

            SetProperty("open?", false);
            SetProperty("openable?", true);

            AddNoun("workshop");
            Short = "workshop door";
            Long = "This metal door has no knob or handle, but appears to swing in both directions.";
        }
    }
}