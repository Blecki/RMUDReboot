using RMUD;

namespace World.Homestead
{
    public class FlowerDoor : MudObject
    {
        public override void Initialize()
        {
            ObjectDecorator.BasicDoor(this);
            AddNoun("flower");
            Short = "flower door";
            Long = "A crudely painted pastel flower decorates this door.";
        }
    }
}