using System;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Luna_BRF
{
    public class CompPowerPlantLuna50T : CompPowerPlant
	{
		public override void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || (this.autoPoweredComp != null && !this.autoPoweredComp.WantsToBeOn) || (this.toxifier != null && !this.toxifier.CanPolluteNow) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				base.PowerOutput = 100f;
				return;
			}
				base.PowerOutput = this.DesiredPowerOutput + 100f;
		}
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
			if (base.Props.soundAmbientProducingPower != null)
			{
				if (base.PowerOutput > 50f)
				{
					if (this.sustainerProducingPower == null || this.sustainerProducingPower.Ended)
					{
						this.sustainerProducingPower = base.Props.soundAmbientProducingPower.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainerProducingPower.Maintain();
					return;
				}
				if (this.sustainerProducingPower != null)
				{
					this.sustainerProducingPower.End();
					this.sustainerProducingPower = null;
				}
			}
		}
		private Sustainer sustainerProducingPower;
	}
}
