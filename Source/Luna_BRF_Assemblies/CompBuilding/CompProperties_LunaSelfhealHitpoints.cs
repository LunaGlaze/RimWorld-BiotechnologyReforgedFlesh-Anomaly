using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_LunaSelfhealHitpoints : CompProperties
	{
		public CompProperties_LunaSelfhealHitpoints()
		{
			this.compClass = typeof(LunaSelfhealHitpoints);
		}

		public int pointPerTimes = 1;

		public int ticksPerTimes;

		//ADD
		public bool requiresFuel;
	}
}
