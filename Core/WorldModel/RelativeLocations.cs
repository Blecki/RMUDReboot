using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    [Flags]
    public enum RelativeLocations
    {
        NONE = 0,
        DEFAULT = 1,

        CONTENTS = 2, //As of a room

        IN = 4,
        ON = 8,
        UNDER = 16,
        BEHIND = 32,

        HELD = 64,
        WORN = 128,

        HIDDEN = 256
    }

    public static class Relloc
    {
        public static String GetRelativeLocationName(RelativeLocations Location)
        {
            if ((Location & RelativeLocations.ON) == RelativeLocations.ON)
                return "on";
            else if ((Location & RelativeLocations.IN) == RelativeLocations.IN)
                return "in";
            else if ((Location & RelativeLocations.UNDER) == RelativeLocations.UNDER)
                return "under";
            else if ((Location & RelativeLocations.BEHIND) == RelativeLocations.BEHIND)
                return "behind";
            else
                return "relloc";
        }
    }
}
