using RMUD;

namespace World.Homestead
{
    public class StormDoor : MudObject
    {
        public override void Initialize()
        {
            AddNoun("DOOR");

            // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
            AddNoun("CLOSED").When(actor => !GetProperty<bool>("open?"));
            AddNoun("OPEN").When(actor => GetProperty<bool>("open?"));

            SetProperty("open?", false);
            SetProperty("openable?", true);

            AddNoun("storm");
            Short = "storm door";
            Long = "Nothing remains of the wire mesh but a few hanging tatters, making this storm door more of an open window.";
        }
    }
}