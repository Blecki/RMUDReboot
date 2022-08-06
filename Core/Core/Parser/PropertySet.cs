using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public class PropertySet : Dictionary<String, Object>
    {
        public PropertySet()
        {
        }

        private PropertySet(Dictionary<String, Object> Arguments)
        {
            foreach (var pair in Arguments)
                this.Upsert(pair.Key, pair.Value);
        }

        public PropertySet Clone()
        {
            return new PropertySet(this);
        }

        /// <summary>
        /// Clone this match, but give the clone an additional argument.
        /// </summary>
        /// <param name="ArgumentName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public PropertySet With(String ArgumentName, Object Value)
        {
            var r = new PropertySet(this);
            r.Upsert(ArgumentName, Value);
            return r;
        }

        /// <summary>
        /// Clone this match, but give the clone many additional arguments.
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public PropertySet With(Dictionary<String, Object> Arguments)
        {
            var r = Clone();
            foreach (var arg in Arguments) r.Upsert(arg.Key, arg.Value);
            return r;
        }
    }
}
