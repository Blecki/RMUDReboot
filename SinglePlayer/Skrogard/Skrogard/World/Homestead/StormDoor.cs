using RMUD;

namespace World.Homestead
{
    public class StormDoor : RMUD.BasicDoor
    {
        public override void Initialize()
        {
            GetProperty<NounList>("nouns").Add("storm");
            Short = "storm door";
            Long = "Nothing remains of the wire mesh but a few hanging tatters, making this storm door more of an open window.";
        }
    }
}