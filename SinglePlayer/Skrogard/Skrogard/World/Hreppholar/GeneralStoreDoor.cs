using RMUD;

namespace World.Hreppholar
{
    public class GeneralStoreDoor : MudObject
    {
        public override void Initialize()
        {
            AddNoun("DOOR");

            // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
            AddNoun("CLOSED").When(actor => !GetProperty<bool>("open?"));
            AddNoun("OPEN").When(actor => GetProperty<bool>("open?"));

            SetProperty("open?", false);
            SetProperty("openable?", true);

            AddNoun("general", "store");
            Short = "General Store Door";
            Long = "A simple door of wood and copper mesh.";
        }
    }
}