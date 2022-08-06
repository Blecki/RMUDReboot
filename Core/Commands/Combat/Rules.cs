using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace RMUD
{
    public class CombatRules 
    {
        [RunAtStartup]
        public static void AtStartup(RMUD.RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("combat target", typeof(MudObject), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("combat weapon", typeof(MudObject), null, new DefaultSerializer());
            PropertyManifest.RegisterProperty("combat health", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("combat damage die", typeof(String), "", new StringSerializer());
            PropertyManifest.RegisterProperty("combat hit modifier", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("combat armor class", typeof(int), 0, new IntSerializer());
            PropertyManifest.RegisterProperty("combat action", typeof(PropertySet), null, new DefaultSerializer()); // Todo: Need better serialization
            PropertyManifest.RegisterProperty("combat pulse counter", typeof(int), 0, new IntSerializer());

            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("attacked", "[Attacker, Target] => Attacker Attacked Target rule.", "Attacker", "Target");
            GlobalRules.DeclarePerformRuleBook<MudObject, MudObject>("melee attack", "[Attacker, Target] => Attacker makes a melee attack on the target.", "Attacker", "Target");

            GlobalRules.Perform<MudObject>("inventory")
                .When(a => a.GetProperty<MudObject>("combat weapon") != null)
                .Do(a =>
                {
                    Core.SendMessage(a, "@wielding", a, a.GetProperty<MudObject>("combat weapon"));
                    return PerformResult.Continue;
                })
                .Name("List weapon in inventory rule.");
            
            GlobalRules.Perform<MudObject, MudObject>("drop")
                .First
                .When((actor, target) => Object.ReferenceEquals(actor.GetProperty<MudObject>("combat weapon"), target))
                .Do((actor, target) =>
            {
                actor.SetProperty("combat weapon", null);
                return PerformResult.Continue;
            }).Name("Unwield dropped item rule");

            GlobalRules.Perform<MudObject, MudObject>("describe")
                .When((viewer, item) => item.GetProperty<MudObject>("combat weapon") != null)
                .Do((viewer, item) =>
                {
                    Core.SendMessage(viewer, "@wielding", item, item.GetProperty<MudObject>("combat weapon"));
                    return PerformResult.Continue;
                })
                .Name("Describe their weapon rule.");

            // Todo: Attack cooldown?
            // Todo: Honor round timeing
            GlobalRules.Perform<MudObject>("heartbeat")
                .When(o => o.HasProperty("combat target") && o.GetProperty<MudObject>("combat target") != null)
                .Do((o) =>
                {
                    var pulseCounter = o.GetProperty<int>("combat pulse counter");
                    if (pulseCounter < Core.SettingsObject.Combat_RoundLength)
                    {
                        o.SetProperty("combat pulse counter", pulseCounter + 1);
                        return PerformResult.Continue;
                    }

                    o.SetProperty("combat pulse counter", 0);

                    var target = o.GetProperty<MudObject>("combat target");
                    if (target == null || target.State == ObjectState.Destroyed)
                    {
                        o.SetProperty("combat target", null);
                        return PerformResult.Continue;
                    }

                    if (!Core.IsVisibleTo(o, target))
                    {
                        Core.SendMessage(o, "Your target is no longer here.");
                        o.SetProperty("combat target", null);
                        return PerformResult.Continue;
                    }

                    var action = o.GetProperty<PropertySet>("combat action");

                    if (action == null || !action.ContainsKey("RULEBOOK"))
                    {
                        action = new PropertySet();
                        action.Add("RULEBOOK", "melee attack");
                        action.Add("ATTACKER", o);
                        action.Add("TARGET", target);
                    }

                    GlobalRules.ConsiderPerformRule(action["RULEBOOK"].ToString(), action["ATTACKER"], action["TARGET"]);                        

                    return PerformResult.Continue;
                })
                .Name("I'm in a fight, I'd better attack rule.");

            GlobalRules.Perform<MudObject, MudObject>("attacked")
                .Do((attacker, target) =>
                {
                    target.SetProperty("combat target", attacker);
                    return PerformResult.Continue;
                })
                .Name("fight back rule");

            GlobalRules.Perform<MudObject, MudObject>("melee attack")
                .Do((attacker, target) =>
                {
                    CombatSystem.MeleeAttack(attacker, target);
                    GlobalRules.ConsiderPerformRule("attacked", attacker, target);
                    return PerformResult.Continue;
                })
                .Name("Perform melee attack rule.");
        }
    }
}
