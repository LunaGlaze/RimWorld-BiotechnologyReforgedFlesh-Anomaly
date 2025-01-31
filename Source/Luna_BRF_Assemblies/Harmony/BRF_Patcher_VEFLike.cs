using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
	//漂浮与游泳生物设置
	[HarmonyPatch(typeof(Pawn_PathFollower))]
	[HarmonyPatch("CostToMoveIntoCell")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	[HarmonyPatch(new Type[]
	{
	typeof(Pawn),
	typeof(IntVec3)
	})]
	public static class BRF_Pawn_PathFollower_CostToMoveIntoCell_Patch_VEFLike
	{
		[HarmonyPostfix]
		public static void DisablePathCostForFloatingCreatures(Pawn pawn, IntVec3 c, ref float __result)
		{
			if (pawn.Map == null)
			{
				return;
			}
			if (LunaHashListCollectionClass.floating_pawns.Contains(pawn))
			{
				TerrainDef terrainDef = pawn.Map.terrainGrid.TerrainAt(c);
				float num = ((c.x != pawn.Position.x && c.z != pawn.Position.z) ? pawn.TicksPerMoveDiagonal : pawn.TicksPerMoveCardinal);
				if (terrainDef == null)
				{
					num = 10000f;
				}
				else if (terrainDef.passability == Traversability.Impassable && !terrainDef.IsWater)
				{
					num = 10000f;
				}
				List<Thing> list = pawn.Map.thingGrid.ThingsListAt(c);
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (thing.def.passability == Traversability.Impassable)
					{
						num = 10000f;
					}
				}
				__result = num;
			}
			if (LunaHashListCollectionClass.waterstriding_pawns.Contains(pawn))
			{
				TerrainDef terrainDef2 = pawn.Map.terrainGrid.TerrainAt(c);
				if (terrainDef2.IsWater)
				{
					float num2 = ((c.x != pawn.Position.x && c.z != pawn.Position.z) ? pawn.TicksPerMoveDiagonal : pawn.TicksPerMoveCardinal);
					__result = num2;
				}
			}
		}
	}
	//可征召生物
	[HarmonyPatch(typeof(FloatMenuMakerMap))]
	[HarmonyPatch("AddJobGiverWorkOrders")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_FloatMenuMakerMap_AddJobGiverWorkOrders_Patch_VEFLike
	{
		[HarmonyPrefix]
		public static bool SkipIfAnimal(Pawn pawn)
		{
			if (LunaHashListCollectionClass.draftable_pawns.Contains(pawn))
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(SchoolUtility))]
	[HarmonyPatch("CanTeachNow")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_SchoolUtility_CanTeachNow_Patch_VEFLike
	{
		[HarmonyPrefix]
		public static bool RemoveTeaching(Pawn teacher)
		{
			if (LunaHashListCollectionClass.draftable_pawns.Contains(teacher))
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(ITab_Pawn_Gear))]
	[HarmonyPatch("ShouldShowEquipment")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_ITab_Pawn_Gear_IsVisible_Patch_VEFLike
	{
		[HarmonyPostfix]
		private static void RemoveTab(Pawn p, ref bool __result)
		{
			//!(p is Machine) ?
			if (LunaHashListCollectionClass.draftable_pawns.Contains(p))
			{
				__result = false;
			}
		}
	}
	[HarmonyPatch(typeof(PawnComponentsUtility))]
	[HarmonyPatch("AddAndRemoveDynamicComponents")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_PawnComponentsUtility_AddAndRemoveDynamicComponents_Patch_VEFLike
	{
		[HarmonyPostfix]
		private static void AddDraftability(Pawn pawn)
		{
			bool flag = pawn.Faction != null && pawn.Faction.IsPlayer;
			bool flag2 = LunaHashListCollectionClass.draftable_pawns.Contains(pawn);
			if (flag && flag2)
			{
				if (pawn.drafter == null)
				{
					pawn.drafter = new Pawn_DraftController(pawn);
				}
				if (pawn.equipment == null)
				{
					pawn.equipment = new Pawn_EquipmentTracker(pawn);
				}
			}
		}
	}
	[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("WorkTypeIsDisabled")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_Pawn_WorkTypeIsDisabled_Patch_VEFLike
	{
		[HarmonyPostfix]
		//&& !(__instance is Machine) ?
		private static void RemoveTendFromAnimals(WorkTypeDef w, Pawn __instance, ref bool __result)
		{
			if (w == WorkTypeDefOf.Doctor && LunaHashListCollectionClass.draftable_pawns.Contains(__instance) && !__instance.RaceProps.IsMechanoid)
			{
				__result = true;
			}
		}
	}
	[HarmonyPatch(typeof(FloatMenuMakerMap))]
	[HarmonyPatch("CanTakeOrder")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_FloatMenuMakerMap_CanTakeOrder_Patch_VEFLike
	{
		[HarmonyPostfix]
		public static void MakePawnControllable(Pawn pawn, ref bool __result)
		{
			bool flag = pawn.Faction != null && (pawn.Faction?.IsPlayer ?? false);
			bool flag2 = LunaHashListCollectionClass.draftable_pawns.Contains(pawn);
			if (flag2 && flag)
			{
				__result = true;
			}
		}
	}
	[HarmonyPatch(typeof(Pawn))]
	[HarmonyPatch("IsColonistPlayerControlled", MethodType.Getter)]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BEF_Pawn_IsColonistPlayerControlled_Patch_VEFLike
	{
		[HarmonyPostfix]
		private static void AddAnimalAsColonist(Pawn __instance, ref bool __result)
		{
			if (LunaHashListCollectionClass.draftable_pawns.Contains(__instance))
			{
				__result = __instance.Spawned && __instance.HostFaction == null && __instance.Faction == Faction.OfPlayer;
			}
		}
	}
	//征召动物nofleeing设置
	[HarmonyPatch(typeof(Pawn_MindState))]
	[HarmonyPatch("StartFleeingBecauseOfPawnAction")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_Pawn_MindState_StartFleeingBecauseOfPawnAction_Patch_VEFLike
	{
		[HarmonyPrefix]
		public static bool DontFlee(Pawn_MindState __instance)
		{
			if (LunaHashListCollectionClass.nofleeing_pawns.Contains(__instance.pawn))
			{
				return false;
			}
			return true;
		}
	}
	[HarmonyPatch(typeof(PawnUtility))]
	[HarmonyPatch("IsFighting")]
	[HarmonyPatchCategory("BRF_VEFLike")]
	public static class BRF_PawnUtility_IsFighting_Patch_VEFLike
	{
		[HarmonyPostfix]
		public static void DontFlee(Pawn pawn, ref bool __result)
		{
			if (pawn != null && LunaHashListCollectionClass.nofleeing_pawns.Contains(pawn) && pawn.CurJob != null)
			{
				__result = true;
			}
		}
	}
}
