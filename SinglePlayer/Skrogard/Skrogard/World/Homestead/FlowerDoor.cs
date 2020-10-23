using RMUD;

namespace World.Homestead
{
    public class FlowerDoor : MudObject
    {
        public override void Initialize()
        {
            AddNoun("DOOR");

            // Doors can be referred to as 'the open door' or 'the closed door' as appropriate.
            AddNoun("CLOSED").When(actor => !GetProperty<bool>("open?"));
            AddNoun("OPEN").When(actor => GetProperty<bool>("open?"));

            SetProperty("open?", false);
            SetProperty("openable?", true);

            AddNoun("flower");
            Short = "flower door";
            Long = "A crudely painted pastel flower decorates this door.";
        }
    }
}