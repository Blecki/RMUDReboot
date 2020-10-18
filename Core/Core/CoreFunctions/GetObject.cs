using System;

namespace RMUD
{
    public partial class Core
    { 
        public static MudObject GetObject(String Path)
        {
            return Core.Database.GetObject(Path);
        }
    }
}
