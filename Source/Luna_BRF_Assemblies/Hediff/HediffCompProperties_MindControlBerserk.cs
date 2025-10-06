using Verse;
using RimWorld;

namespace Luna_BRF
{
    public class HediffCompProperties_MindControlBerserk : HediffCompProperties
	{
		public HediffCompProperties_MindControlBerserk()
		{
			compClass = typeof(HediffComp_MindControlBerserk);
		}
		public MentalStateDef mentalStateDef;
		public FactionDef factionDef;
        public bool mandatory = false;
    }
}
