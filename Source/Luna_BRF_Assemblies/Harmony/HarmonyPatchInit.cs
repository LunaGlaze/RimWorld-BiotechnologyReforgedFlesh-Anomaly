using System;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Luna_BRF
{
	[StaticConstructorOnStartup]
	public class HarmonyPatchInit
	{
		private static Harmony BRFHarmony;
		static HarmonyPatchInit()
		{
			BRFHarmony = new Harmony("lunaglaze.Anomaly.BioReforgedFlesh");
			if (ModsConfig.AnomalyActive)
			{
				BRFHarmony.Patch((MethodBase)AccessTools.PropertyGetter(typeof(HediffComp_Invisibility), "ForcedVisible"), (HarmonyMethod)null, new HarmonyMethod(typeof(BRF_Patcher), "ForcedVisiblePostfix", (Type[])null), (HarmonyMethod)null, (HarmonyMethod)null);
			}
			BRFHarmony.PatchAll();
		}
	}
}