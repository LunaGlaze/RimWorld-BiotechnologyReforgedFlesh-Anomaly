using UnityEngine;
using Verse;
namespace Luna_BRF
{
	public class Hediff_BRFRapidRegeneration : Hediff
	{
		private float hpRemaining => (this.Severity * 1000);

		private float hpCapacity = 1000f;

		public override string SeverityLabel => string.Format("{0:0} / {1:0}{2}", hpRemaining, hpCapacity, "HP".Translate());


		public void SetHpCapacity(float amount)
		{
			Severity = (hpCapacity = amount)/1000;
		}

		public override void Notify_Regenerated(float hp)
		{
			Severity = Mathf.Max(hpRemaining - hp, 0f)/1000;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look(ref hpCapacity, "hpCapacity", 0f);
		}
	}
}
