using Verse;

namespace Luna_BRF
{
	public class HediffCompProperties_FleshbeastEmergeLittle : HediffCompProperties
	{
		[MustTranslate]
		public string letterLabel;

		[MustTranslate]
		public string letterText;

		public HediffCompProperties_FleshbeastEmergeLittle()
		{
			compClass = typeof(HediffComp_FleshbeastEmergeLittle);
		}
	}
}
