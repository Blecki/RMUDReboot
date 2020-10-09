using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace RMUD
{
    public static partial class Core
    {
        public static String StripColorTags(String In)
        {
            var r = "";
            var readingColor = false;
            foreach (var c in In)
            {
                if (readingColor)
                {
                    if (c == ';')
                        readingColor = false;
                }
                else if (c == '$')
                    readingColor = true;
                else
                    r += c;
            }
            return r;
        }
    }
}
