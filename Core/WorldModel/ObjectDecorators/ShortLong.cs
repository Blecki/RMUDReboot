using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class MudObject
	{
        public String Short
        {
            get { return GetProperty<String>("short"); }
            set { SetProperty("short", value); }
        }

        public String Long
        {
            get { return GetProperty<String>("long"); }
            set { SetProperty("long", value); }
        }
    }
}
