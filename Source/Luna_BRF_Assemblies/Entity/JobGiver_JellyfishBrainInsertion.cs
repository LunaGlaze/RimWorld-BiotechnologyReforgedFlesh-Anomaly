using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    class JobGiver_JellyfishBrainInsertion : JobGiver_AICastAbility
	{
		protected override LocalTargetInfo GetTarget(Pawn pawn, Ability ability)
		{
			List<Pawn> pawns = new List<Pawn>();
			foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (item.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("BRF_ScarletCerebralJellyfishBrainInsertion")) == null && !item.AnimalOrWildMan() && item.health.hediffSet.GetBrain() != null && item.Faction != pawn.Faction && item.health.State != PawnHealthState.Down)
				{
					pawns.Add(item);
				}
			}
            if (!pawns.NullOrEmpty())
			{
				float num = float.MaxValue;
				Pawn result = null;
				foreach (Pawn item in pawns)
				{
					if (pawn.Position.InHorDistOf(item.Position, 25f) && (float)item.Position.DistanceToSquared(pawn.Position) < num && GenSight.LineOfSightToThing(pawn.Position, item, pawn.Map))
					{
						num = item.Position.DistanceToSquared(pawn.Position);
						result = item;
					}
				}
				if(result != null)
				{
					return new LocalTargetInfo(result);
				}
				Random rnd = new Random();
				int randIndex = rnd.Next(pawns.Count);
				Pawn target = pawns[randIndex];
				return new LocalTargetInfo(target);

			}
			return null;
		}
	}
}
