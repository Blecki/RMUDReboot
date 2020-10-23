using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public partial class MudObject
    {
        public void Actor()
        {
            Container(RelativeLocations.HELD | RelativeLocations.WORN, RelativeLocations.HELD);

            SetProperty("actor?", true);
            SetProperty("preserve?", true);

            AddNoun("MAN").When((a) => a.GetProperty<Gender>("gender") == RMUD.Gender.Male);
            AddNoun("WOMAN").When((a) => a.GetProperty<Gender>("gender") == RMUD.Gender.Female);
        }

    }
}
