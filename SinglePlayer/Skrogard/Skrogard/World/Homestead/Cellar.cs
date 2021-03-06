﻿using RMUD;

namespace World.Homestead
{

    public class Cellar : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);

            SetProperty("short", "Cellar");
            SetProperty("long", "Carved from the rock, this room is amazingly cool. The walls are very slightly damp, and the floor uneven. Shelves, empty save the dust, line the walls.");

            OpenLink(Direction.UP, "Homestead.SittingRoom");
            OpenLink(Direction.EAST, "Homestead.Balcony", Core.GetObject("Homestead.StormDoor@inside"));

        }
    }
}