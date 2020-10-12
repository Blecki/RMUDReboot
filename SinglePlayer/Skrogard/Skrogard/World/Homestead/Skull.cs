using RMUD;

namespace World.Homestead
{
    public class Skull : RMUD.MudObject
    {
        public override void Initialize()
        {
            Short = "child's skull";
            AddNoun("CHILD", "CHILDS", "SKULL");
        }
    }
}