using Verse;

namespace Luna_BRF
{
	public class CompProperties_Draftable : CompProperties
	{
		public bool makeNonFleeingToo = false;

		public CompProperties_Draftable()
		{
			compClass = typeof(CompDraftable);
		}
	}
}
