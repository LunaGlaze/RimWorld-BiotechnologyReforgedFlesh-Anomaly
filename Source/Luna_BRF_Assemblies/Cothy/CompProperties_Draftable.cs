using Verse;

namespace Luna_BRF
{
	public class CompProperties_Draftable : CompProperties
    {
        public bool makeNonFleeingToo = false;

        public bool conditionalOnTrainability = false;

        public int checkingInterval = 500;

        public CompProperties_Draftable()
		{
			compClass = typeof(CompDraftable);
		}
	}
}
