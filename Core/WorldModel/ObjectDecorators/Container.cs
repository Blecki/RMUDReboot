using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class MudObject
    {
        public void Container(RelativeLocations Locations, RelativeLocations Default)
        {
            ContentLocationsAllowed = Locations;
            DefaultContentLocation = Default;
            Contents = new Dictionary<RelativeLocations, List<MudObject>>();

            SetProperty("container?", true);
        }        
    }
}
