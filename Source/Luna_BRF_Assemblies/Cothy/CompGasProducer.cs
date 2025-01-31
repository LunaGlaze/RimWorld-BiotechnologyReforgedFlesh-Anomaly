using System;
using Verse;

namespace Luna_BRF
{
	internal class CompGasProducer : ThingComp
	{
		private int gasProgress = 0;

		private int gasTickMax = 64;

		private Random rand = new Random();

		public CompProperties_GasProducer Props => (CompProperties_GasProducer)props;

		public override void CompTick()
		{
			gasProgress++;
			if (gasProgress <= gasTickMax)
			{
				return;
			}
			Pawn pawn = parent as Pawn;
			if (pawn.Map == null)
			{
				return;
			}
			if (!Props.generateIfDowned || (Props.generateIfDowned && !pawn.Downed && !pawn.Dead))
			{
				foreach (IntVec3 cell in GenAdj.OccupiedRect(pawn.Position, pawn.Rotation, IntVec2.One).ExpandedBy(Props.radius).Cells)
				{
					if (cell.InBounds(pawn.Map) && rand.NextDouble() < (double)Props.rate)
					{
						Thing thing = ThingMaker.MakeThing(ThingDef.Named(Props.gasType));
						thing.Rotation = Rot4.North;
						thing.Position = cell;
						thing.SpawnSetup(pawn.Map, false);
					}
				}
			}
			gasProgress = 0;
		}
	}
}
