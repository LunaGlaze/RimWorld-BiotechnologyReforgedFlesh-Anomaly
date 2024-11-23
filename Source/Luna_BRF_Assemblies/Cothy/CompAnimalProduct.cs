using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
	public class CompAnimalProduct : CompHasGatherableBodyResource
	{
		public int seasonalItemIndex = 0;

		private System.Random rand = new System.Random();

		protected override int GatherResourcesIntervalDays => Props.gatheringIntervalDays;

		protected override int ResourceAmount => Props.resourceAmount;

		protected override ThingDef ResourceDef
		{
			get
			{
				if (Props.seasonalItems != null)
				{
					return ThingDef.Named(Props.seasonalItems[seasonalItemIndex]);
				}
				if (Props.isRandom)
				{
					return ThingDef.Named(Props.randomItems.RandomElement());
				}
				return Props.resourceDef;
			}
		}

		protected override string SaveKey => "resourceGrowth";

		public CompProperties_AnimalProduct Props => (CompProperties_AnimalProduct)props;

		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				return !(parent is Pawn pawn) || pawn.ageTracker.CurLifeStage.shearable;
			}
		}

		public override string CompInspectStringExtra()
		{
			if (!Active)
			{
				return null;
			}
			if (Props.hideDisplayOnWildAnimals && parent?.Faction != Faction.OfPlayerSilentFail)
			{
				return null;
			}
			if (!Props.customResourceString.NullOrEmpty())
			{
				return Props.customResourceString.Translate() + ": " + base.Fullness.ToStringPercent();
			}
			return "ResourceGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos && parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set to produce now",
					defaultDesc = "Sets animal products to be ready to be gathered now",
					action = delegate
					{
						fullness = 1f;
					}
				};
			}
		}

		public void InformGathered(Pawn doer)
		{
			if (!Active)
			{
				Log.Error(doer?.ToString() + " gathered body resources while not Active: " + parent);
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield)))
			{
				MoteMaker.ThrowText((doer.DrawPos + parent.DrawPos) / 2f, parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int num = GenMath.RoundRandom((float)ResourceAmount * fullness);
				while (num > 0)
				{
					int num2 = Mathf.Clamp(num, 1, ResourceDef.stackLimit);
					num -= num2;
					Thing thing = ThingMaker.MakeThing(ResourceDef);
					thing.stackCount = num2;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near);
				}
				if (Props.hasAditional && rand.NextDouble() <= (double)((float)Props.additionalItemsProb / 100f))
				{
					if (Props.goInOrder)
					{
						foreach (string item in Props.additionalItems.InRandomOrder())
						{
							ThingDef namedSilentFail = DefDatabase<ThingDef>.GetNamedSilentFail(item);
							if (namedSilentFail != null)
							{
								Thing thing2 = ThingMaker.MakeThing(ThingDef.Named(Props.additionalItems.RandomElement()));
								thing2.stackCount = Props.additionalItemsNumber;
								GenPlace.TryPlaceThing(thing2, doer.Position, doer.Map, ThingPlaceMode.Near);
							}
						}
					}
					else
					{
						Thing thing3 = ThingMaker.MakeThing(ThingDef.Named(Props.additionalItems.RandomElement()));
						thing3.stackCount = Props.additionalItemsNumber;
						GenPlace.TryPlaceThing(thing3, doer.Position, doer.Map, ThingPlaceMode.Near);
					}
				}
			}
			fullness = 0f;
		}
	}
}
