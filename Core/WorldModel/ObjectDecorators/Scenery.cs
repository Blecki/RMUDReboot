using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class MudObject
	{
        public MudObject AddScenery(String Description, params String[] Nouns)
		{
			var scenery = new MudObject();
            scenery.SetProperty("scenery?", true);
            scenery.SetProperty("long", Description);
			foreach (var noun in Nouns)
				scenery.AddNoun(noun.ToUpper());
            AddScenery(scenery);
            return scenery;
		}

        public void AddScenery(MudObject Scenery)
        {
            Scenery.SetProperty("scenery?", true);
            Add(Scenery, RelativeLocations.CONTENTS);
            Scenery.Location = this;
        }
    }
}
