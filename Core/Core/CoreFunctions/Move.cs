using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace RMUD
{
    public partial class Core
    {
        /// <summary>
        /// Move Object to Destination.
        /// </summary>
        /// <param name="Object">Object to move.</param>
        /// <param name="Destination">Destination to move too.</param>
        /// <param name="Location">The relative location within destination to move too.</param>
        public static void Move(MudObject Object, MudObject Destination, RelativeLocations Location = RelativeLocations.DEFAULT)
        {
            if (Object == null) return;

            if (Object.Location.HasValue(out var loc))
                loc.Remove(Object);

            if (Destination != null)
                Destination.Add(Object, Location);
            Object.Location = Destination;
        }
    }
}
