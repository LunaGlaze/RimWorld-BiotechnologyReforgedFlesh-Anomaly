using RimWorld;
using System;
using Verse;

namespace Luna_BRF
{
    public class CompProperties_DropButcherProductsDead : CompProperties
	{
		public CompProperties_DropButcherProductsDead()
		{
			this.compClass = typeof(CompLuna_DropButcherProductsDead);
		}
		public ThingDef specialDropWithSize;
		public float minBodySize = 0;
		public float perBodySize;
		public int perCount = 1;
	}
}