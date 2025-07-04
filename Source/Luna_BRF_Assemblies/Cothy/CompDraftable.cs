using RimWorld;
using Verse;

namespace Luna_BRF
{
	internal class CompDraftable : ThingComp
    {
        public int tickCounter = 0;

        public CompProperties_Draftable Props => (CompProperties_Draftable)props;

        public override void CompTick()
        {
            tickCounter--;
            if (tickCounter >= 0)
            {
                return;
            }
            Pawn pawn = parent as Pawn;
            if (Props.conditionalOnTrainability)
            {
                LunaHashListCollectionClass.RemoveDraftablePawnFromList(parent);
                if (Props.makeNonFleeingToo)
                {
                    LunaHashListCollectionClass.RemoveNotFleeingPawnFromList(parent);
                }
            }
            else
            {
                if (pawn.drafter == null)
                {
                    pawn.drafter = new Pawn_DraftController(pawn);
                }
                if (pawn.equipment == null)
                {
                    pawn.equipment = new Pawn_EquipmentTracker(pawn);
                }
                LunaHashListCollectionClass.AddDraftablePawnToList(parent);
                if (Props.makeNonFleeingToo)
                {
                    LunaHashListCollectionClass.AddNotFleeingPawnToList(parent);
                }
            }
            tickCounter = Props.checkingInterval;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LunaHashListCollectionClass.AddDraftablePawnToList(parent);
			if (Props.makeNonFleeingToo)
			{
				LunaHashListCollectionClass.AddNotFleeingPawnToList(parent);
			}
		}

		public override void PostDeSpawn(Map map, DestroyMode mode)
		{
			LunaHashListCollectionClass.RemoveDraftablePawnFromList(parent);
			if (Props.makeNonFleeingToo)
			{
				LunaHashListCollectionClass.RemoveNotFleeingPawnFromList(parent);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			LunaHashListCollectionClass.RemoveDraftablePawnFromList(parent);
			if (Props.makeNonFleeingToo)
			{
				LunaHashListCollectionClass.RemoveNotFleeingPawnFromList(parent);
			}
		}
	}
}
