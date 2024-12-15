using Verse;
using RimWorld;

namespace Luna_BRF
{
	public class HediffCompProperties_PawnSpanwDeath : HediffCompProperties
	{
		public bool giveLetter = false;

		[MustTranslate]
		public string letterLabel;

		[MustTranslate]
		public string letterText;

		public PawnKindDef spanwPawnKind;

		public EntityCodexEntryDef codexEntry;

		public float minActSeverity = 1f;

		public float severityFeduce = 1f;

		public ThingDef filth;

		public int filthCount = 3;

		public FactionDef factionDef;

		public bool sameFaction = false;

		public bool manHunter = false;

		public HediffCompProperties_PawnSpanwDeath()
		{
			compClass = typeof(HediffComp_PawnSpanwDeath);
		}

	}
}
