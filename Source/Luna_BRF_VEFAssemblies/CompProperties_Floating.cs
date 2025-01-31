using Verse;

namespace Luna_BRF_VEFAssemblies
{
	public class CompProperties_Floating : CompProperties
	{
		public bool isFloater = true;

		public bool canCrossWater = false;

		public CompProperties_Floating()
		{
			compClass = typeof(CompFloating);
		}
	}
}
