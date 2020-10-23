﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class MudObject
	{
        public void Locale(Locale Type)
        {
            Container(RelativeLocations.CONTENTS, RelativeLocations.CONTENTS);
            SetProperty("locale type", Type);
        }

        public void OpenLink(Direction Direction, String Destination, MudObject Portal = null)
        {
            if (RemoveAll(thing => thing.GetProperty<Direction>("link direction") == Direction && thing.GetProperty<bool>("portal?")) > 0)
                Core.LogWarning("Opened duplicate link in " + Path);

            if (Portal == null)
            {
                Portal = new MudObject();
                Portal.SetProperty("link anonymous?", true);
                Portal.SetProperty("short", "link " + Direction + " to " + Destination);
            }

            Portal.SetProperty("portal?", true);
            Portal.SetProperty("link direction", Direction);
            Portal.SetProperty("link destination", Destination);
            Portal.Location = this;
            Add(Portal, RelativeLocations.CONTENTS);
        }
    }
}
