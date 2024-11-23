using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_LunaDamagedWhenUnfueled : CompProperties
	{
		public CompProperties_LunaDamagedWhenUnfueled()
		{
			this.compClass = typeof(LunaDamagedWhenUnfueled);
		}
		public int damage;
		public int interval;
	}
}
