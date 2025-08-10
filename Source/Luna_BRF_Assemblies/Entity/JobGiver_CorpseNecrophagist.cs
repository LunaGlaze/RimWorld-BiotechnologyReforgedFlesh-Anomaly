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
                if (TryGetAnimalTarget(pawn, ability, out var targetInfo))
                {
                    return targetInfo;
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
				if(TryGetAnimalTarget(pawn,ability,out var targetInfo))
				{
					return targetInfo;
                }
			}
			return null;
        }
        protected bool TryGetAnimalTarget(Pawn pawn, Ability ability, out LocalTargetInfo targetInfo)
        {
            List<Pawn> animal = new List<Pawn>();
            foreach (Pawn item in pawn.Map.mapPawns.AllPawnsSpawned)
            {
                if (item.IsAnimal && item.kindDef.combatPower < PawnKindDefOf.Thrumbo.combatPower && item.Faction != ability.pawn.Faction &&
                    pawn.Map.reachability.CanReach(pawn.Position, item.SpawnedParentOrMe, PathEndMode.OnCell, TraverseParms.For(pawn, canBashDoors: true, canBashFences: true)))
                {
                    animal.Add(item);
                }
            }
            if (animal.Count > 0)
            {
                Pawn i = animal.RandomElement();
				targetInfo = new LocalTargetInfo(i);
                return true;
            }
			targetInfo = null;
			return false;
        }


    }
}
