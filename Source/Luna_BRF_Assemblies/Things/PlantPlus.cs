using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    [StaticConstructorOnStartup]
    public class PlantPlus : Plant
    {
		public float GrowthRateFactor_NoxiousHazePlus
		{
			get
			{
				if (NoxiousHazeUtility.IsExposedToNoxiousHaze(this))
				{
					return 0.9f;
				}
				return 1f;
			}
		}
		public float GrowthRateFactor_TemperaturePlus
		{
			get
			{
				if (!GenTemperature.TryGetTemperatureForCell(base.Position, base.Map, out var tempResult))
				{
					return 1f;
				}
				if (tempResult < -50f)
				{
					return Mathf.InverseLerp(-65f, -50f, tempResult);
				}
				if (tempResult > 99f)
				{
					return Mathf.InverseLerp(128f, 99f, tempResult);
				}
				return 1f;
			}
		}
		public float GrowthRateFactor_LightPlus
		{
			get
			{
				float glow = base.Map.glowGrid.GroundGlowAt(base.Position);
				return Mathf.Clamp01(PlantUtility.GrowthRateFactorFor_Light(def, glow)*1.25f + 0.15f);
			}
		}
		public override float GrowthRate
		{
			get
			{
				if (Blighted)
				{
					return 0f;
				}
				return GrowthRateFactor_Fertility * GrowthRateFactor_LightPlus * GrowthRateFactor_NoxiousHazePlus * GrowthRateFactor_TemperaturePlus;
			}
		}
		public override string GrowthRateCalcDesc
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (GrowthRateFactor_Fertility != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("FertilityLower".Translate()) + ": " + GrowthRateFactor_Fertility.ToStringPercent());
				}
				if (GrowthRateFactor_TemperaturePlus != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("TemperatureLower".Translate()) + ": " + GrowthRateFactor_TemperaturePlus.ToStringPercent());
				}
				if (GrowthRateFactor_LightPlus != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate("LightLower".Translate()) + ": " + GrowthRateFactor_LightPlus.ToStringPercent());
				}
				if (ModsConfig.BiotechActive && base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.NoxiousHaze) && GrowthRateFactor_NoxiousHazePlus != 1f)
				{
					stringBuilder.AppendInNewLine("StatsReport_MultiplierFor".Translate(GameConditionDefOf.NoxiousHaze.label) + ": " + GrowthRateFactor_NoxiousHazePlus.ToStringPercent());
				}
				return stringBuilder.ToString();
			}
		}
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (def.plant.showGrowthInInspectPane)
			{
				if (LifeStage == PlantLifeStage.Growing)
				{
					stringBuilder.AppendLine("PercentGrowth".Translate(GrowthPercentString));
					stringBuilder.AppendLine("GrowthRate".Translate() + ": " + GrowthRate.ToStringPercent());
					if (!base.Blighted)
					{
						if (Resting)
						{
							stringBuilder.AppendLine("PlantResting".Translate());
						}
						if (!HasEnoughLightToGrow)
						{
							stringBuilder.AppendLine("PlantNeedsLightLevel".Translate() + ": " + def.plant.growMinGlow.ToStringPercent());
						}
						float growthRateFactor_Temperature = GrowthRateFactor_TemperaturePlus;
						if (growthRateFactor_Temperature < 0.99f)
						{
							if (growthRateFactor_Temperature < 0.01f)
							{
								stringBuilder.AppendLine("OutOfIdealTemperatureRangeNotGrowing".Translate());
							}
							else
							{
								stringBuilder.AppendLine("OutOfIdealTemperatureRange".Translate(Mathf.Max(1, Mathf.RoundToInt(growthRateFactor_Temperature * 100f)).ToString()));
							}
						}
					}
				}
				else if (LifeStage == PlantLifeStage.Mature)
				{
					if (HarvestableNow)
					{
						stringBuilder.AppendLine("ReadyToHarvest".Translate());
					}
					else
					{
						stringBuilder.AppendLine("Mature".Translate());
					}
				}
				if (DyingBecauseExposedToLight)
				{
					stringBuilder.AppendLine("DyingBecauseExposedToLight".Translate());
				}
				if (Blighted)
				{
					stringBuilder.AppendLine("Blighted".Translate() + " (" + Blight.Severity.ToStringPercent() + ")");
				}
			}
			string text = InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}
		public override void TickLong()
		{
			CheckMakeLeafless();
			if (base.Destroyed)
			{
				return;
			}
			float num = growthInt;
			bool num2 = LifeStage == PlantLifeStage.Mature;
			growthInt += base.GrowthPerTick * 2000f;
			if (growthInt > 1f)
			{
				growthInt = 1f;
			}
			if (((!num2 && LifeStage == PlantLifeStage.Mature) || (int)(num * 10f) != (int)(growthInt * 10f)) && CurrentlyCultivated())
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
			if (!HasEnoughLightToGrow)
			{
				unlitTicks += 2000;
			}
			else
			{
				unlitTicks = 0;
			}
			ageInt += 2000;
			if (base.Dying)
			{
				Map map = base.Map;
				bool isCrop = base.IsCrop;
				bool harvestableNow = HarvestableNow;
				bool dyingBecauseExposedToLight = DyingBecauseExposedToLight;
				int num3 = Mathf.CeilToInt(CurrentDyingDamagePerTick * 2000f);
				TakeDamage(new DamageInfo(DamageDefOf.Rotting, num3));
				if (base.Destroyed && isCrop && def.plant.Harvestable && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfRot-" + def.defName, 240f))
				{
					string key = (harvestableNow ? "MessagePlantDiedOfRot_LeftUnharvested" : ((!dyingBecauseExposedToLight) ? "MessagePlantDiedOfRot" : "MessagePlantDiedOfRot_ExposedToLight"));
					Messages.Message(key.Translate(GetCustomLabelNoCount(includeHp: false)).CapitalizeFirst(), new TargetInfo(base.Position, map), MessageTypeDefOf.NegativeEvent);
				}
			}
		}
		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(DestroyMode mode)
		{
			foreach (ThingDefCountClass additionalLeaving in base.GetAdditionalLeavings(mode))
			{
				yield return additionalLeaving;
			}
			ExtensionDef_PlantPlusAdditionalLeavings ExtraDropped = this.def.GetModExtension<ExtensionDef_PlantPlusAdditionalLeavings>();
            foreach (ExtensionDef_PlantPlusAdditionalLeavings.PlantPlusExtraDropped x in ExtraDropped.extraDropped) {
				yield return new ThingDefCountClass(x.extraDroppedThing, Mathf.RoundToInt(x.extraDroppedCount * Growth));
			}
		}

	}

}
