using HarmonyLib;
using RimWorld;
using System;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    //反隐
    public class BRF_HediffComp_Invisibility_Patcher
    {
        [HarmonyPostfix]
        public static void ForcedVisiblePostfix(ref bool __result, HediffComp_Invisibility __instance)
		{
			if (__instance.Pawn.health.hediffSet.HasHediff(LunaDefOf.BRF_InvisibleWatched))
			{
				__result = true;
			}
		}
    }
	//猎杀人类排除部分目标

    [HarmonyPatch(typeof(JobGiver_Manhunter), "FindPawnTarget")]
    public static class BRF_JobGiver_Manhunter_FindPawnTarget_Patcher
    {
        static void Postfix(ref Pawn __result, Pawn pawn, bool canBashFences, bool canBashDoors)
        {
            if (flag(__result))
            {
                Pawn newpawn = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, 
                    TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, 
                    (Thing x) => x is Pawn y && (int)x.def.race.intelligence >= 1 && !flag(y),
                    0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors, canTakeTargetsCloserThanEffectiveMinRange: true, canBashFences);
                __result = newpawn;
            }
        }
        static bool flag(Pawn pawn)
        {
            ExtensionDef_LunaDisallowManhunterTarget pawnProps = pawn.def.GetModExtension<ExtensionDef_LunaDisallowManhunterTarget>();
            if(pawnProps != null) { return pawnProps.disallowFindPawnTargetManhunter; }
            else { return pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("BRF_BloodstainedTickParasiticed")) != null; }
        }
    }
	//根据工作速率调整燃料消耗
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
