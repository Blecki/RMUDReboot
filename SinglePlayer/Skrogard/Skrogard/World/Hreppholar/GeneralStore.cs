﻿using RMUD;

namespace World.Hreppholar
{
    public class GeneralStore : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "General Store");
            SetProperty("long", "Dusty shelves stand in neat rows, filled with goods of all kinds. New mixed with old - clean pieces of glass ware and canned goods, old iron pots, clothes both tattered and new hanging on long rods. A counter stands at the back.");

            OpenLink(Direction.NORTH, "Hreppholar.GeneralStorePorch", Core.GetObject("Hreppholar.GeneralStoreDoor@inside"));
            OpenLink(Direction.SOUTH, "Hreppholar.GeneralStoreBackRoom");

            Core.Move(Core.GetObject("Hreppholar.Bjorn"), this);
        }
    }
}