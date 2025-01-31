using Verse;

namespace Luna_BRF
{
	public class CompFloating : ThingComp
	{
		public CompProperties_Floating Props => (CompProperties_Floating)props;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (Props.isFloater)
			{
				LunaHashListCollectionClass.AddFloatingPawnToList(parent);
			}
            if (Props.canCrossWater)
			{
				LunaHashListCollectionClass.AddWaterstridingPawnToList(parent);
			}
		}

		public override void PostDeSpawn(Map map)
		{
			if (Props.isFloater)
			{
				LunaHashListCollectionClass.RemoveFloatingPawnFromList(parent);
			}
			if (Props.canCrossWater)
			{
				LunaHashListCollectionClass.RemoveWaterstridingPawnFromList(parent);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (Props.isFloater)
			{
				LunaHashListCollectionClass.RemoveFloatingPawnFromList(parent);
			}
			if (Props.canCrossWater)
			{
				LunaHashListCollectionClass.RemoveWaterstridingPawnFromList(parent);
			}
		}
	}
}
