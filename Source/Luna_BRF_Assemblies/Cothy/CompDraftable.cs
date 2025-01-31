using Verse;

namespace Luna_BRF
{
	internal class CompDraftable : ThingComp
	{
		public CompProperties_Draftable Props => (CompProperties_Draftable)props;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LunaHashListCollectionClass.AddDraftablePawnToList(parent);
			if (Props.makeNonFleeingToo)
			{
				LunaHashListCollectionClass.AddNotFleeingPawnToList(parent);
			}
		}

		public override void PostDeSpawn(Map map)
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
