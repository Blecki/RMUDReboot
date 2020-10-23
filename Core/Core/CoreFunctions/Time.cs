using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public enum LightingLevel
    {
        Bright = 2,
        Dim = 1,
        Dark = 0
    }

    public partial class Core
    {
        /// <summary>
        /// Sophisticated celestial calculations. Results guaranteed to be accurate approximately 50% of the time.
        /// </summary>
        public static bool IsDay { get { return true; } }
        public static bool IsNight { get { return false; } }

        public static DateTime TimeOfDay = DateTime.Parse("03/15/2015 11:15:00 -5:00");
    }
}
