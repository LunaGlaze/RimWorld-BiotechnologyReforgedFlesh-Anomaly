using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
	[StaticConstructorOnStartup]
	public class HarmonyPatchInit
	{
		private static HarmonyLib.Harmony BRFHarmony;
		static HarmonyPatchInit()
		{
			BRFHarmony = new HarmonyLib.Harmony("lunaglaze.Anomaly.BiotechnologyReforgedFlesh");
			BRFHarmony.PatchAllUncategorized();
			if (ModsConfig.AnomalyActive)
			{
				BRFHarmony.Patch((MethodBase)AccessTools.PropertyGetter(typeof(HediffComp_Invisibility), "ForcedVisible"), (HarmonyMethod)null, new HarmonyMethod(typeof(BRF_HediffComp_Invisibility_Patcher), "ForcedVisiblePostfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
			}
			if (!LunaHashListCollectionClass.VEFloaded)
			{
				BRFHarmony.PatchCategory("BRF_VEFLike");
			}
		}
	}
}