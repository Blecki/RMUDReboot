using System;

namespace RMUD
{
    public partial class Core
    { 
        public static MudObject InitializeObject(MudObject Object)
        {
            Object.Initialize();
            Object.State = ObjectState.Alive;
            Core.GlobalRules.ConsiderPerformRule("update", Object);
            return Object;
        }
    }
}
