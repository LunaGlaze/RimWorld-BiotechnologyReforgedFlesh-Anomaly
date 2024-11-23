using Verse;

namespace Luna_BRF
{
	public class CompProperties_InvisibleWatcher : CompProperties
	{

		public float range;

		public int checkIntervalTicks;

		public bool requiresFuel;

		public bool requiresPower;

		public CompProperties_InvisibleWatcher()
		{
			compClass = typeof(LunaInvisibleWatcherCom);
		}
	}
}