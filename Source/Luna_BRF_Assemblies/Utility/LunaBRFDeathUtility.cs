using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public static class LunaBRFDeathUtility
	{
		public static IEnumerable<ThingDefCountClass> GetPawnBaseButcherProductsThingDefCountClass(Pawn pawn, float efficiency = 1f)
		{
			if (pawn.RaceProps.meatDef != null)
			{
				int num = GenMath.RoundRandom(pawn.GetStatValue(StatDefOf.MeatAmount) * efficiency);
				if (num > 0)
				{
					yield return new ThingDefCountClass(pawn.RaceProps.meatDef, num);
				}
			}
			if (pawn.RaceProps.leatherDef != null)
			{
				int num2 = GenMath.RoundRandom(pawn.GetStatValue(StatDefOf.LeatherAmount) * efficiency);
				if (num2 > 0)
				{
					yield return new ThingDefCountClass(pawn.RaceProps.leatherDef, num2);
				}
			}
			if (pawn.def.butcherProducts != null)
			{
				for (int i = 0; i < pawn.def.butcherProducts.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = pawn.def.butcherProducts[i];
					int num = GenMath.RoundRandom((float)thingDefCountClass.count * efficiency);
					if (num > 0)
					{
						yield return new ThingDefCountClass(thingDefCountClass.thingDef, num);
					}
				}
			}
		}
		public static IEnumerable<Thing> GetPawnBaseButcherProductsThing(Pawn pawn, float efficiency = 1f)
		{
			List<ThingDefCountClass> baseButcherProducts = new List<ThingDefCountClass>();
			baseButcherProducts.AddRange(GetPawnBaseButcherProductsThingDefCountClass(pawn, efficiency));
			if (baseButcherProducts != null)
			{
				for (int i = 0; i < baseButcherProducts.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = baseButcherProducts[i];
					if (thingDefCountClass.count > 0)
					{
						Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef);
						thing.stackCount = thingDefCountClass.count;
						yield return thing;
					}
				}
			}
			if (pawn.RaceProps.Humanlike)
			{
				yield break;
			}
			PawnKindLifeStage lifeStage = pawn.ageTracker.CurKindLifeStage;
			if (lifeStage.butcherBodyPart == null || (pawn.gender != 0 && (pawn.gender != Gender.Male || !lifeStage.butcherBodyPart.allowMale) && (pawn.gender != Gender.Female || !lifeStage.butcherBodyPart.allowFemale)))
			{
				yield break;
			}
			while (true)
			{
				BodyPartRecord bodyPartRecord = pawn.health.hediffSet.GetNotMissingParts().FirstOrDefault((BodyPartRecord x) => x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup));
				if (bodyPartRecord != null)
				{
					pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, pawn, bodyPartRecord));
					yield return ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing ?? bodyPartRecord.def.spawnThingOnRemoved);
					continue;
				}
				break;
			}
		}
	}
}