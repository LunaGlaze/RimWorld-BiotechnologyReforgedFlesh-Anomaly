using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class JobGiver_CorpseNecrophagist : JobGiver_AICastAbility
	{
		protected override LocalTargetInfo GetTarget(Pawn pawn, Ability ability)
		{
			if(pawn.TryGetComp<CompNecrophagistEntity>(out var comp))
            {
                if (comp.Digesting)
                {
					return null;
                }
            }
			List<Thing> corpses = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				.Where(thing => thing is Corpse corpse && corpse.InnerPawn != null && corpse.InnerPawn.RaceProps.IsFlesh).ToList();
			if(Rand.Bool && pawn.Map.mapPawns.AllPawnsSpawned != null)
			{
				foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (item.AnimalOrWildMan() && item.kindDef.combatPower < 500 && item.Faction != ability.pawn.Faction)
					{
						return new LocalTargetInfo(item);
					}
				}
			}
			if (corpses.Count > 0)
			{
				Thing closestValidCorpse = GenClosest.ClosestThing_Global_Reachable(
					pawn.Position,
					pawn.Map,
					corpses,
					PathEndMode.OnCell,
					TraverseParms.For(pawn)
				);
				if (closestValidCorpse != null)
				{
					return new LocalTargetInfo(closestValidCorpse);
				}
			}
			if (pawn.Map.mapPawns.AllPawnsSpawned != null)
			{
				foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (item.health.State == PawnHealthState.Down && item.RaceProps.IsFlesh && item.Faction != ability.pawn.Faction)
					{
						return new LocalTargetInfo(item);
					}
					if (item.AnimalOrWildMan() && item.kindDef.combatPower <= 500 && item.Faction != ability.pawn.Faction)
					{
						return new LocalTargetInfo(item);
					}
				}
			}
			return null;
		}
	}
}
