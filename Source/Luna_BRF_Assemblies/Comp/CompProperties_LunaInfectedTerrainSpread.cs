using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_LunaInfectedTerrainSpread : CompProperties
	{
		public CompProperties_LunaInfectedTerrainSpread()
		{
			this.compClass = typeof(LunaInfectedTerrainSpread);
		}

		public float radius;

		public IntRange spawnTickRate;

		public TerrainDef requiredTerrain;

		public TerrainDef terrainToSet;

		public float baseWorkingSpeedMultiplier = 1f;

		public List<TerrainDef> allowTerrains = null;

	}
}
