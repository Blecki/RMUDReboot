using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public static class RoomLightingRules 
    {
        [RunAtStartup]
        public static void AtStartup(RuleEngine GlobalRules)
        {
            GlobalRules.Perform<MudObject>("update")
                .When(room => room.GetProperty<Locale>("locale type") != Locale.NotARoom)
                .Do(room =>
                {
                    var light = LightingLevel.Dark;
                    var roomType = room.GetProperty<Locale>("locale type");

                    if (roomType == RMUD.Locale.Exterior)
                        light = Core.SettingsObject.AmbientExteriorLightingLevel;

                    foreach (var item in Core.EnumerateVisibleTree(room))
                    {
                        var lightingLevel = item.GetProperty<LightingLevel>("emissive light");
                        if (lightingLevel > light) light = lightingLevel;
                    }

                    var ambient = room.GetProperty<LightingLevel>("ambient light");
                    if (ambient > light) light = ambient;

                    room.SetProperty("locale light", light);

                    return PerformResult.Continue;
                })
                .Name("Update locale lighting rule.");
        }
    }
}
