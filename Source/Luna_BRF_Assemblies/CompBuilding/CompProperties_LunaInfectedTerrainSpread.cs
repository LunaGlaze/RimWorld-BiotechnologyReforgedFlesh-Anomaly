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
        public class SpecialConvertTerrainDefSet
        {
            public TerrainDef requiredTerrainDef;
            public TerrainDef convertTerrainDef;
        }

        public float radius;

		public IntRange spawnTickRate;

		public TerrainDef setConvertTerrainDef;

		public float baseWorkingSpeedMultiplier = 1f;

		public List<TerrainDef> allowTerrains = null;

        public List<SpecialConvertTerrainDefSet> specialConvertTerrainDefs = new List<SpecialConvertTerrainDefSet>();

    }
}
