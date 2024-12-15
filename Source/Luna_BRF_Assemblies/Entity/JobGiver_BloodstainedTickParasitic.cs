using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class JobGiver_BloodstainedTickParasitic : JobGiver_AICastAbility
	{
		protected override LocalTargetInfo GetTarget(Pawn pawn, Ability ability)
		{
			List<Thing> corpses = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				.Where(thing => thing is Corpse corpse && corpse.InnerPawn != null && corpse.InnerPawn.RaceProps.FleshType == FleshTypeDefOf.Insectoid).ToList();
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
			return null;
		}
	}
}
