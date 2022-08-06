using System;
using System.Collections.Generic;

namespace RMUD
{
    public static class CombatSystem
    {
        public static String DefaultWeapon = "";

        public static MudObject GetDefaultWeapon()
        {
            if (String.IsNullOrEmpty(DefaultWeapon))
                return new MudObject();
            else
                return Core.GetObject(DefaultWeapon);
        }

        public static void StartFight(MudObject Attacker, MudObject Target)
        {
            var existingTarget = Attacker.GetProperty<MudObject>("combat target");
            if (existingTarget != null)
                return; // Already have a target.
            Attacker.SetProperty("combat target", Target);
        }

        public static void MeleeAttack(MudObject Attacker, MudObject Target)
        {
            if (!Target.HasProperty("combat health"))
                return;

            MudObject weapon = Attacker.GetProperty<MudObject>("combat weapon");
            if (weapon == null)
            {
                weapon = GetDefaultWeapon();
                weapon.Location = Attacker;
            }

            var hitModifier = weapon.GetProperty<int>("combat hit modifier");
            var armorClass = Target.GetProperty<int>("combat armor class");

            Core.SendMessage(Attacker, "Hit mod " + hitModifier + " vs AC " + armorClass);
            var hitRoll = Core.RollDice("1d20") + hitModifier;
            
            
            if (hitRoll == 20 || hitRoll >= armorClass)
            {
                var damageDie = weapon.GetProperty<String>("combat damage die");
                var damage = Core.RollDice(damageDie);
                Core.SendMessage(Attacker, "Hit for (" + damageDie + ") - " + damage + " damage!");
                if (hitRoll == 20)
                {
                    var additionalDamage = Core.RollDice(damageDie);
                    Core.SendMessage(Attacker, "Critical! " + additionalDamage + " additional damage!");
                    damage += additionalDamage;
                }

                Core.SendLocaleMessage(Attacker, "^<0> hits ^<1> for " + damage + " damage!", Attacker, Target);
            }
            else
            {
                Core.SendMessage(Attacker, "@you miss", Attacker, Target);
                Core.SendLocaleMessage(Attacker, "@they miss", Attacker, Target);
            }
        }
    }
}
