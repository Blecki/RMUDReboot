using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public static class RegisterActorProperties
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("actor?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("preserve?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("gender", typeof(Gender), Gender.Male, new EnumSerializer<Gender>());
            PropertyManifest.RegisterProperty("rank", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("client", typeof(Client), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("command handler", typeof(ClientCommandHandler), null, new DefaultSerializer());
        }
    }

    public partial class MudObject
    {
        public void Actor()
        {
            Container(RelativeLocations.Held | RelativeLocations.Worn, RelativeLocations.Held);

            SetProperty("actor?", true);
            SetProperty("preserve?", true);

            AddNoun("MAN").When((a) => a.GetProperty<Gender>("gender") == RMUD.Gender.Male);
            AddNoun("WOMAN").When((a) => a.GetProperty<Gender>("gender") == RMUD.Gender.Female);
        }

    }
}
