using Verse;

namespace Luna_BRF
{
	public class CompProperties_GasProducer : CompProperties
	{
		public string gasType = "";

		public float rate = 0f;

		public int radius = 0;

		public bool generateIfDowned = false;

		public CompProperties_GasProducer()
		{
			compClass = typeof(CompGasProducer);
		}
	}
}
