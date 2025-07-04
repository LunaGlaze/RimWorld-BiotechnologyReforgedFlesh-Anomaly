using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_LunaInfectedPlantsSpread : CompProperties
	{
		public CompProperties_LunaInfectedPlantsSpread()
		{
			this.compClass = typeof(LunaInfectedPlantsSpread);
		}

		public float radius;

		public IntRange spawnTickRate;

		public TerrainDef requiredTerrain;

		public ThingDef plant;

	}
}
