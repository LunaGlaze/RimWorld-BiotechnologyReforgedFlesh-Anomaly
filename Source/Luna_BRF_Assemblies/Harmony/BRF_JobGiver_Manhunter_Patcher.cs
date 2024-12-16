using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

namespace Luna_BRF.Harmony
{
    [HarmonyPatch(typeof(JobGiver_Manhunter) , "FindPawnTarget")]
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

}
