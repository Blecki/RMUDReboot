using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMUD;

namespace RMUD
{
    public class CombatMessages 
    {
        [RunAtStartup]
        public static void AtStartup(RuleEngine GlobalRules)
        {
            Core.StandardMessage("you attack uselessly", "You attack <the1>, but it doesn't do a damn thing.");
            Core.StandardMessage("they attack uselessly", "^<the0> attacks <the1>, but it doesn't do a damn thing.");
            Core.StandardMessage("you wield", "You get your <1> ready to attack.");
            Core.StandardMessage("they wield", "^<the0> gets their <1> ready to attack.");
            Core.StandardMessage("wielding", "Wielding: <a1>");
        }
    }
}
