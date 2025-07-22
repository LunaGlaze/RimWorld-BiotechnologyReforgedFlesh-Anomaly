using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class JobGiver_CorpseNecrophagist : JobGiver_AICastAbilityWithBashDoors
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
				.Where(thing => thing is Corpse corpse && corpse.InnerPawn != null && corpse.InnerPawn.RaceProps.IsFlesh && corpse.InnerPawn.def.defName != "BRF_RawProphet").ToList();
			if(Rand.Bool && pawn.Map.mapPawns.AllPawnsSpawned != null)
			{
				List<Pawn> animal = new List<Pawn>();
				foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (item.IsAnimal && item.kindDef.combatPower < 500 && item.Faction != ability.pawn.Faction)
					{
						animal.Add(item);
					}
				}
				if (animal.Count > 0)
				{
					Thing closestValidAnimal = GenClosest.ClosestThing_Global_Reachable(
						pawn.Position,
						pawn.Map,
						animal,
						PathEndMode.OnCell,
						TraverseParms.For(pawn, canBashDoors: true, canBashFences: true)
					);
					if (closestValidAnimal != null)
					{
						return new LocalTargetInfo(closestValidAnimal);
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
					TraverseParms.For(pawn, canBashDoors: true, canBashFences: true)
				);
				if (closestValidCorpse != null)
				{
					return new LocalTargetInfo(closestValidCorpse);
				}
			}
			if (pawn.Map.mapPawns.AllPawnsSpawned != null)
			{
				List<Pawn> animal = new List<Pawn>();
				foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
				{
					if (item.IsAnimal && item.kindDef.combatPower < 500 && item.Faction != ability.pawn.Faction &&
						pawn.Map.reachability.CanReach(pawn.Position,item.SpawnedParentOrMe, PathEndMode.OnCell, TraverseParms.For(pawn, canBashDoors: true, canBashFences: true)))
					{
						animal.Add(item);
					}
				}
				if (animal.Count > 0)
				{
					Pawn i = animal.RandomElement();
					return new LocalTargetInfo(i);
				}
			}
			return null;
		}
	}
}
