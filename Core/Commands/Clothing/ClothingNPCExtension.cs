﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RMUD;

namespace RMUD
{
	public static class ClothingNPCExtension
	{
        public static void Wear(this MudObject NPC, MudObject Item)
        {
            Core.Move(Item, NPC, RelativeLocations.WORN);
        }

        public static void Wear(this MudObject NPC, String Short, ClothingLayer Layer, ClothingBodyPart BodyPart)
        {
            Wear(NPC, ClothingFactory.Create(Short, Layer, BodyPart));
        }
	}
}
