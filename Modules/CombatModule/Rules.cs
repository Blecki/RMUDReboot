using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace CombatModule
{
    public class CombatRules 
    {
        [RunAtStartup]
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("combat_weapon", typeof(MudObject), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("combat_health", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("combat_damage_die", typeof(String), "", new StringSerializer());
            PropertyManifest.RegisterProperty("combat_hit_modifier", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("combat_armor_class", typeof(int), 0, new IntSerializer());

            GlobalRules.Perform<MudObject>("inventory")
                .When(a => a.GetProperty<MudObject>("combat_weapon") != null)
                .Do(a =>
                {
                    Core.SendMessage(a, "@wielding", a, a.GetProperty<MudObject>("combat_weapon"));
                    return PerformResult.Continue;
                })
                .Name("List weapon in inventory rule.");
            
            GlobalRules.Perform<MudObject, MudObject>("drop")
                .First
                .When((actor, target) => Object.ReferenceEquals(actor.GetProperty<MudObject>("combat_weapon"), target))
                .Do((actor, target) =>
            {
                actor.SetProperty("combat_weapon", null);
                return PerformResult.Continue;
            }).Name("Unwield dropped item rule");

            GlobalRules.Perform<MudObject, MudObject>("describe")
                .When((viewer, item) => item.GetProperty<MudObject>("combat_weapon") != null)
                .Do((viewer, item) =>
                {
                    Core.SendMessage(viewer, "@wielding", item, item.GetProperty<MudObject>("combat_weapon"));
                    return PerformResult.Continue;
                })
                .Name("Describe their weapon rule.");
        }
    }
}
