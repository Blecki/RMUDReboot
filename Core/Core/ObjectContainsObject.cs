using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace RMUD
{
    public partial class MudObject
    {
        /// <summary>
        /// Determine if an object is contained by another.
        /// Note that nothing prevents an object containing itself, however, this function will report
        ///  false for any loop in the containment graph.
        /// </summary>
        /// <param name="Super">Reference point object</param>
        /// <param name="Sub">Object to be tested</param>
        /// <returns></returns>
        public static bool ObjectContainsObject(MudObject Super, MudObject Sub)
        {
            if (Object.ReferenceEquals(Super, Sub)) return false; //Objects can't contain themselves...
            if (!Sub.Location.HasValue(out var loc)) return false;
            if (Object.ReferenceEquals(Super, loc)) return true;
            return ObjectContainsObject(Super, loc);
        }
    }
}
