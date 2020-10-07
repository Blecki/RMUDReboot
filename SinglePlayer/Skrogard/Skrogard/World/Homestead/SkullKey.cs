using RMUD;

namespace World.Homestead
{
    public class SkullKey : RMUD.MudObject
    {
        public override void Initialize()
        {
            Short = "skull key";
            GetProperty<NounList>("nouns").Add("KEY", "SKULL");
        }
    }
}