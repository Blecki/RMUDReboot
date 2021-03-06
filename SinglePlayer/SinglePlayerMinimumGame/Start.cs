﻿using RMUD;

namespace Minimum
{

    public class Start : RMUD.MudObject
    {
        public override void Initialize()
        {
            Locale(RMUD.Locale.Exterior);
            SetProperty("short", "Start Room");
            SetProperty("long", "This is a game with the minimum possible objects.");
        }
    }   
}