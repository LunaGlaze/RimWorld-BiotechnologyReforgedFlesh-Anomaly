using AnimalBehaviours;
using Verse;

namespace Luna_BRF_VEFAssemblies
{
	public class CompFloating : ThingComp
	{
		public CompProperties_Floating Props => (CompProperties_Floating)props;

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (Props.isFloater)
			{
				AnimalCollectionClass.AddFloatingAnimalToList(parent);
			}
            if (Props.canCrossWater)
			{
				AnimalCollectionClass.AddWaterstridingPawnToList(parent);
			}
		}

		public override void PostDeSpawn(Map map)
		{
			if (Props.isFloater)
			{
				AnimalCollectionClass.RemoveFloatingAnimalFromList(parent);
			}
			if (Props.canCrossWater)
			{
				AnimalCollectionClass.RemoveWaterstridingPawnFromList(parent);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (Props.isFloater)
			{
				AnimalCollectionClass.RemoveFloatingAnimalFromList(parent);
			}
			if (Props.canCrossWater)
			{
				AnimalCollectionClass.RemoveWaterstridingPawnFromList(parent);
			}
		}
	}
}
