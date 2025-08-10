using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using static HarmonyLib.Code;

namespace Luna_BRF
{
    //反隐
    public class BRF_HediffComp_Invisibility_Patcher
    {
        [HarmonyPostfix]
        public static void ForcedVisiblePostfix(ref bool __result, HediffComp_Invisibility __instance)
        {
            if (__instance.Pawn.health.hediffSet.HasHediff(LunaBRFDefOf.BRF_InvisibleWatched))
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
            if (pawnProps != null) { return pawnProps.disallowFindPawnTargetManhunter; }
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
    //根据营养值补充燃料
    [HarmonyPatch(typeof(CompRefuelable), "Refuel")]
    [HarmonyPatch(new Type[] { typeof(List<Thing>) })]
    public static class BRF_CompRefuelable_Patcher_RefuelByNutrition
    {
        static void Prefix(ref List<Thing> fuelThings, CompRefuelable __instance)
        {
            CompRefuelable baseComp = __instance;
            ExtensionDef_RefuelByNutrition refuelByNutrition = __instance.parent.def.GetModExtension<ExtensionDef_RefuelByNutrition>();
            if (refuelByNutrition != null)
            {
                if (refuelByNutrition.refuelByNutrition)
                {
                    float nutritionMultiplier = refuelByNutrition.nutritionMultiplier;
                    if (__instance.Props.atomicFueling && fuelThings.Sum((Thing t) => t.stackCount) < __instance.GetFuelCountToFullyRefuel())
                    {
                        Log.ErrorOnce("Error refueling; not enough fuel available for proper atomic refuel", 19586442);
                        return;
                    }
                    int num = __instance.GetFuelCountToFullyRefuel();
                    while (num > 0 && fuelThings.Count > 0)
                    {
                        Thing thing = fuelThings.Pop();
                        float nutrition = thing.GetStatValue(StatDefOf.Nutrition);
                        int num2 = Mathf.FloorToInt(Mathf.Min(num, thing.stackCount * nutrition * nutritionMultiplier));
                        int count = Mathf.Min(Mathf.CeilToInt(num2 / nutrition / nutritionMultiplier), thing.stackCount);
                        __instance.Refuel(num2);
                        thing.SplitOff(count).Destroy();
                        num -= num2;
                    }
                    return;
                }
            }
        }
    }
}