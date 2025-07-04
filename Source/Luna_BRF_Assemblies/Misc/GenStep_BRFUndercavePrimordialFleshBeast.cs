using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class GenStep_BRFUndercavePrimordialFleshBeast : GenStep
    {
        public override int SeedPart => 26098423;

		public override void Generate(Map map, GenStepParams parms)
		{
			Thing pitGateExit = map.listerThings.ThingsOfDef(ThingDef.Named("PitGateExit")).FirstOrDefault();
			Pawn dreadmeld = map.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => p.kindDef == PawnKindDefOf.Dreadmeld);
			int randomInRange = PrimordialFleshBeastPointCountRange.RandomInRange + anomalyNum;
			List<IntVec3> interestPoints = new List<IntVec3>();
			for (int i = 0; i < randomInRange; i++)
			{
				if (CellFinder.TryFindRandomCell(map, delegate (IntVec3 c)
				{
					if (!c.Standable(map))
					{
						return false;
					}
					if (c.DistanceToEdge(map) <= 5)
					{
						return false;
					}
					if (pitGateExit != null && c.InHorDistOf(pitGateExit.Position, 10f))
					{
						return false;
					}
					if (dreadmeld != null && c.InHorDistOf(dreadmeld.Position, 10f))
					{
						return false;
					}
					return !interestPoints.Any((IntVec3 p) => c.InHorDistOf(p, 10f));
				}, out var result))
				{
					interestPoints.Add(result);
				}
			}
			foreach (IntVec3 item in interestPoints)
			{
				foreach (IntVec3 item2 in GridShapeMaker.IrregularLump(item, map, 20))
				{
					foreach (Thing item3 in item2.GetThingList(map).ToList())
					{
						if (item3.def.destroyable && ((item2.GetEdifice(map)?.def?.building?.isNaturalRock ?? false) || item2.GetEdifice(map)?.def == ThingDefOf.Fleshmass))
						{
							item3.Destroy();
						}
					}
				}
				GenerateSleepingPrimordialFleshbeasts(map, item);
			}
		}
		private void GenerateSleepingPrimordialFleshbeasts(Map map, IntVec3 cell)
		{
			int randomInRange = NumFleshbeastsRange.RandomInRange + anomalyNum;
			for (int i = 0; i < randomInRange; i++)
			{
				Pawn newThing = PawnGenerator.GeneratePawn(LunaDefOf.BRF_PrimordialFleshBeast, Faction.OfEntities);
				if (CellFinder.TryFindRandomCellNear(cell, map, 4, (IntVec3 c) => c.Standable(map) && c.GetFirstPawn(map) == null, out var result) && GenSpawn.Spawn(newThing, result, map).TryGetComp(out CompCanBeDormant comp))
				{
					comp.ToSleep();
				}
			}
		}
		private static readonly IntRange NumFleshbeastsRange = new IntRange(2,  4);
		private static readonly IntRange PrimordialFleshBeastPointCountRange = new IntRange(1, 3);
        private int anomalyNum => Find.Anomaly.LevelDef.anomalyThreatTier>0 ? Find.Anomaly.LevelDef.anomalyThreatTier : 0 ;
    }
}
