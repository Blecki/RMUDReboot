using RMUD;

namespace World.Homestead
{
    public class FlowerDoor : RMUD.BasicDoor
    {
        public override void Initialize()
        {
            GetProperty<NounList>("nouns").Add("flower");
            Short = "flower door";
            Long = "A crudely painted pastel flower decorates this door.";
        }
    }
}