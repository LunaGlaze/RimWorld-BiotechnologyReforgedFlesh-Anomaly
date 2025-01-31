using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Luna_BRF
{
	public static class LunaBRFHediffUtility
	{
		public static int CountAddedAndImplantedPartsFlesh(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
                    if (hediffs[i].def.organicAddedBodypart || hediffs[i].def.tags.Contains("BRF_FleshReforge"))
					{
						num++;
					}
					else if (hediffs[i].TryGetComp<HediffComp_FleshbeastEmerge>(out var comp0) || hediffs[i].TryGetComp<HediffComp_FleshbeastEmergeLittle>(out var comp1))
					{
						num++;
					}
					else if (hediffs[i].def.spawnThingOnRemoved != null)
                    {
						ThingDef partThing = hediffs[i].def.spawnThingOnRemoved;
						if(partThing.thingCategories?.Contains(ThingCategoryDef.Named("BodyPartsNatural")) != false || partThing.thingCategories?.Contains(DefDatabase<ThingCategoryDef>.GetNamedSilentFail("AA_ImplantCategory")) != false || partThing.thingCategories?.Contains(DefDatabase<ThingCategoryDef>.GetNamedSilentFail("VFEI_BodyPartsInsect")) != false || partThing.thingCategories?.Contains(DefDatabase<ThingCategoryDef>.GetNamedSilentFail("GR_ImplantCategory")) != false )
						{
							if(partThing.thingCategories?.Contains(ThingCategoryDef.Named("BRF_BodyPartsNaturalHuman")) == false)
							{
								num++;
							}
						}
					}
				}
			}
			return num;
		}
		public static IEnumerable<BodyPartRecord> GetLungWithoutFleshReforgeBodyParts(Pawn pawn)
		{
			return from p in pawn.health.hediffSet.GetNotMissingParts()
				   where p.def == BodyPartDefOf.Lung && !pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == p && x.def.tags.Contains("BRF_FleshReforge"))
				   select p;
		}
	}
}
