using System;
using RimWorld;
using Verse;

namespace Luna_BRF
{
	// Token: 0x02000028 RID: 40
	public class LunaDamagedWhenUnfueled : ThingComp
	{
		public CompProperties_LunaDamagedWhenUnfueled Props
		{
			get
			{
				return (CompProperties_LunaDamagedWhenUnfueled)this.props;
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.refuelComp = this.parent.GetComp<CompRefuelable>();
		}

		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(this.Props.interval) && this.refuelComp != null && !this.refuelComp.HasFuel)
			{
				ThingWithComps parent = this.parent;
				int hitPoints = parent.HitPoints;
				parent.HitPoints = hitPoints - 1;
				if (this.parent.HitPoints <= 0)
				{
					this.parent.Kill(null, null);
				}
			}
		}

		private CompRefuelable refuelComp;
	}
}
