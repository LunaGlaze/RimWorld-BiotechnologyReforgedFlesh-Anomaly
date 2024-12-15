using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Luna_BRF
{
	public class CompProperties_TransmuteFlesh : CompProperties_AbilityEffect
	{
		public class TFElementRatio
		{
			public ThingFilter fixedSourceFilter;

			public float ratio = 1f;
		}
		public class SpecialTransmuteList
		{
			public ThingDef specialInput;
			public ThingDef specialOutput;
		}

		public List<TFElementRatio> elementRatios = new List<TFElementRatio>();

		public List<SpecialTransmuteList> specialTransmuteList = new List<SpecialTransmuteList>();

		public List<ThingDef> outcomeItems = new List<ThingDef>();

		[MustTranslate]
		public string failedMessage;

		public CompProperties_TransmuteFlesh()
		{
			compClass = typeof(CompAbilityEffect_TransmuteFlesh);
		}
	}
}
