using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public static class ObjectProperties
    {
        [RunAtStartup]
        public static void AtStartup(RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("container?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("locale type", typeof(Locale), Locale.NotARoom, new EnumSerializer<Locale>());
            PropertyManifest.RegisterProperty("locale light", typeof(LightingLevel), LightingLevel.Dark, new EnumSerializer<LightingLevel>());
            PropertyManifest.RegisterProperty("ambient light", typeof(LightingLevel), LightingLevel.Dark, new EnumSerializer<LightingLevel>());
            PropertyManifest.RegisterProperty("actor?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("preserve?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("gender", typeof(Gender), Gender.Male, new EnumSerializer<Gender>());
            PropertyManifest.RegisterProperty("rank", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("client", typeof(Client), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("command handler", typeof(ClientCommandHandler), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("emissive light", typeof(LightingLevel), LightingLevel.Dark, new EnumSerializer<LightingLevel>());
            PropertyManifest.RegisterProperty("locked?", typeof(bool), true, new BoolSerializer());
            PropertyManifest.RegisterProperty("scenery?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("short", typeof(String), "unnamed object", new StringSerializer());
            PropertyManifest.RegisterProperty("long", typeof(String), "", new StringSerializer());
            PropertyManifest.RegisterProperty("article", typeof(String), "a", new StringSerializer());
            PropertyManifest.RegisterProperty("nouns", typeof(NounList), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("openable?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("open?", typeof(bool), false, new BoolSerializer());
            PropertyManifest.RegisterProperty("combat target", typeof(MudObject), null, new DefaultSerializer());

        }
    }
}