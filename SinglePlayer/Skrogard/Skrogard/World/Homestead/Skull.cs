using RMUD;

namespace World.Homestead
{
    public class Skull : RMUD.MudObject
    {
        public override void Initialize()
        {
            Short = "child's skull";
            GetProperty<NounList>("nouns").Add("CHILD", "CHILDS", "SKULL");
        }
    }
}