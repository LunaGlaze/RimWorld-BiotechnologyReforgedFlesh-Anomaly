using RimWorld;
using System;
using Verse;

namespace Luna_BRF
{
	public class LunaSelfhealHitpoints : ThingComp
	{
		public CompProperties_LunaSelfhealHitpoints Props
		{
			get
			{
				return (CompProperties_LunaSelfhealHitpoints)this.props;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassedSinceLastHeal, "ticksPassedSinceLastHeal", 0, false);
		}
		public bool HasFuel
		{
			get
			{
				CompRefuelable comp = this.parent.GetComp<CompRefuelable>();
				return comp != null && comp.HasFuel;
			}
		}
		public int minTickInterval => Math.Max(this.Props.ticksPerTimes,50);
		public override void CompTick()
		{
			if (parent.IsHashIntervalTick(minTickInterval))
			{
				this.Tick(minTickInterval);
			}
		}

		public override void CompTickRare()
		{
			this.Tick(250);
		}

		public override void CompTickLong()
		{
			this.Tick(2000);
		}

		private void Tick(int ticks)
		{
			if (this.Props.requiresFuel && !this.HasFuel)
			{
				return;
			}
			this.ticksPassedSinceLastHeal += ticks;
			if (this.ticksPassedSinceLastHeal >= this.Props.ticksPerTimes)
			{
				this.ticksPassedSinceLastHeal = 0;
				if (this.parent.HitPoints < this.parent.MaxHitPoints)
				{
					ThingWithComps parent = this.parent;
					int hitPoints = parent.HitPoints;
					parent.HitPoints = Math.Min(hitPoints + this.Props.pointPerTimes, this.parent.MaxHitPoints);
				}
			}
		}

		public int ticksPassedSinceLastHeal;
	}
}