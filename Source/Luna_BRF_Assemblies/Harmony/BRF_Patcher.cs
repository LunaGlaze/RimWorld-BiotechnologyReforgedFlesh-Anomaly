using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class BRF_Patcher
    {
		public static void ForcedVisiblePostfix(ref bool __result, HediffComp_Invisibility __instance)
		{
			if (__instance.Pawn.health.hediffSet.HasHediff(LunaDefOf.BRF_InvisibleWatched))
			{
				__result = true;
			}
		}

	}
}
