using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace CombatModule
{
    public class HitModifierRuleType
    {
        public int HitModifier = 0;
    }

    public class ArmorClassRuleType
    {
        public int ArmorClass = 0;
    }

    public static class System
    {
        public static Random Random = new Random();
        public static DiceGrammar DiceGrammar = new DiceGrammar();
        public static String DefaultWeapon = "";

        // Keep track of active combats
        // Update them on heartbeats

        public static int RollDice(String Dice)
        {
            var itr = new Ancora.StringIterator(Dice);
            var r = DiceGrammar.Root.Parse(itr);

            if (r.ResultType == Ancora.ResultType.Success)
                return DiceGrammar.CalculateDieRoll(r.Node, Random);
            else
                return 1;
        }

        public static MudObject GetDefaultWeapon()
        {
            if (String.IsNullOrEmpty(DefaultWeapon))
                return new MudObject();
            else
                return MudObject.GetObject(DefaultWeapon);
        }

        public static void MeleeAttack(MudObject Attacker, MudObject Target)
        {
            if (!Target.HasProperty("combat_health"))
                return;

            MudObject weapon = Attacker.GetProperty<MudObject>("combat_weapon");
            if (weapon == null)
            {
                weapon = GetDefaultWeapon();
                weapon.Location = Attacker;
            }

            var hitModifier = weapon.GetProperty<int>("combat_hit_modifier");
            var armorClass = Target.GetProperty<int>("combat_armor_class");

            Core.SendMessage(Attacker, "Hit mod " + hitModifier + " vs AC " + armorClass);
            var hitRoll = RollDice("1d20") + hitModifier;
            
            
            if (hitRoll == 20 || hitRoll >= armorClass)
            {
                var damageDie = weapon.GetProperty<String>("combat_damage_die");
                var damage = RollDice(damageDie);
                Core.SendMessage(Attacker, "Hit for (" + damageDie + ") - " + damage + " damage!");
                if (hitRoll == 20)
                {
                    var additionalDamage = RollDice(damageDie);
                    Core.SendMessage(Attacker, "Critical! " + additionalDamage + " additional damage!");
                    damage += additionalDamage;
                }
            }
            else
            {
                Core.SendMessage(Attacker, "@you miss", Attacker, Target);
                Core.SendLocaleMessage(Attacker, "@they miss", Attacker, Target);
            }
        }
    }
}
