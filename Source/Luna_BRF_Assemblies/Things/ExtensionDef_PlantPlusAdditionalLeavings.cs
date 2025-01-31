using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class ExtensionDef_PlantPlusAdditionalLeavings : DefModExtension
	{
		public class PlantPlusExtraDropped
		{
			public ThingDef extraDroppedThing;
			public int extraDroppedCount;
		}
		public List<PlantPlusExtraDropped> extraDropped = new List<PlantPlusExtraDropped>();
	}
}
