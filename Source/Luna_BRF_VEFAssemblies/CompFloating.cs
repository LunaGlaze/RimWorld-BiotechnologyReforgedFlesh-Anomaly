using VEF.AnimalBehaviours;
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
                StaticCollectionsClass.AddFloatingAnimalToList(parent);
			}
            if (Props.canCrossWater)
			{
                StaticCollectionsClass.AddWaterstridingPawnToList(parent);
			}
		}

		public override void PostDeSpawn(Map map, DestroyMode mode)
		{
			if (Props.isFloater)
			{
				StaticCollectionsClass.RemoveFloatingAnimalFromList(parent);
			}
			if (Props.canCrossWater)
			{
				StaticCollectionsClass.RemoveWaterstridingPawnFromList(parent);
			}
		}

		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (Props.isFloater)
			{
				StaticCollectionsClass.RemoveFloatingAnimalFromList(parent);
			}
			if (Props.canCrossWater)
			{
				StaticCollectionsClass.RemoveWaterstridingPawnFromList(parent);
			}
		}
	}
}
