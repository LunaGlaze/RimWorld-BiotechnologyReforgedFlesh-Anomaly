using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class BRF_HediffComp_Invisibility_Patcher
	{
		public static void ForcedVisiblePostfix(ref bool __result, HediffComp_Invisibility __instance)
		{
			if (__instance.Pawn.health.hediffSet.HasHediff(LunaDefOf.BRF_InvisibleWatched))
			{
				__result = true;
			}
		}
    }

    [HarmonyPatch(typeof(JobGiver_Manhunter), "FindPawnTarget")]
    public static class BRF_JobGiver_Manhunter_FindPawnTarget_Patcher
    {
        static void Postfix(ref Pawn __result, Pawn pawn, bool canBashFences, bool canBashDoors)
        {
            if (__result.def == ThingDef.Named("BRF_BloodstainedTick") || __result.health?.hediffSet?.GetFirstHediffOfDef(LunaDefOf.BRF_ScarletFever) != null || __result.health?.hediffSet?.GetFirstHediffOfDef(LunaDefOf.BRF_BloodstainedTickParasiticed) != null)
            {
                Pawn newpawn = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn y && (int)x.def.race.intelligence >= 1 && x.def != ThingDef.Named("BRF_BloodstainedTick") && y.health?.hediffSet?.GetFirstHediffOfDef(LunaDefOf.BRF_BloodstainedTickParasiticed) == null, 0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors, canTakeTargetsCloserThanEffectiveMinRange: true, canBashFences);
                __result = newpawn;
            }
        }
    }
    [HarmonyPatch(typeof(CompRefuelable), nameof(CompRefuelable.ConsumeFuel))]
    public static class BRF_CompRefuelable_Patcher
    {
        static void Prefix(ref float amount, CompRefuelable __instance)
        {
            LunaInfectedTerrainSpread lunaComp = __instance.parent.GetComp<LunaInfectedTerrainSpread>();
            if (lunaComp != null)
            {
                amount = amount * lunaComp.workingSpeedMultiplier;
            }
        }
    }
}
