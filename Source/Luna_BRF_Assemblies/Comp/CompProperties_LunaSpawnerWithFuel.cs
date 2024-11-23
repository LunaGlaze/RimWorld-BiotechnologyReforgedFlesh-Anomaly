using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_LunaSpawnerWithFuel : CompProperties
	{
		public CompProperties_LunaSpawnerWithFuel()
		{
			this.compClass = typeof(LunaSpawnerWithFuel);
		}

		public ThingDef thingToSpawn;

		public int spawnCount = 1;

		public IntRange spawnIntervalRange = new IntRange(100, 100);

		public int spawnMaxAdjacent = -1;

		public bool spawnForbidden;

		public bool requiresPower;

		public bool writeTimeLeftToSpawn;

		public bool showMessageIfOwned;

		public string saveKeysPrefix;

		public bool inheritFaction;

		//ADD
		public bool requiresFuel;

		public bool playSound;

		public SoundDef sound;

	}
}
