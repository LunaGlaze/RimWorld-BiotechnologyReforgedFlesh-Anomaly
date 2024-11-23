using System;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class LunaDeadChanceSplit : ThingComp
    {
        private CompProperties_DeadChanceSplit Props => (CompProperties_DeadChanceSplit)this.props;
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			base.Notify_Killed(prevMap, dinfo);
			if (!ModLister.CheckAnomaly("Pawn dividing"))
			{
				return;
			}
			Pawn innerPawn = this.parent as Pawn;
			if (innerPawn == null)
			{
                return;
			}
			float rand = Rand.Value;
			if (rand <= Props.DeadSplitChance)
			{
				int dividePawnCount = Props.dividePawnCount;
				Lord prevLord = innerPawn.GetLord();
				for (int i = 0; i < dividePawnCount; i++)
				{
					Pawn child = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Props.dividePawnKindOptions.RandomElement(), innerPawn.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, 0f));
					SpawnPawn(child, innerPawn, innerPawn.PositionHeld, innerPawn.MapHeld, prevLord);
				}
			}
		}
		private void SpawnPawn(Pawn child, Pawn parent, IntVec3 position, Map map, Lord lord)
		{
			GenSpawn.Spawn(child, position, map, WipeMode.VanishOrMoveAside);
			lord?.AddPawn(child);
			CompInspectStringEmergence compInspectStringEmergence = child.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = parent;
			}
			FleshbeastUtility.SpawnPawnAsFlyer(child, map, position);
		}
	}
}
