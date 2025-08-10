using System;
using RimWorld;
using Verse;
namespace Luna_BRF
{
	public class ElectrostaticBuilding : Building
	{
		public override void TickRare()
		{
			base.TickRare();
			CompRefuelable comp = this.GetComp<CompRefuelable>();
            if ((comp != null && comp.HasFuel) || comp == null)
			{
				FleckMaker.Static(this.Position, this.Map, LunaBRFDefOf.BlastEMP);
			}
		}
		protected override void Tick()
		{
			base.Tick();
            if (this.IsHashIntervalTick(250))
			{
				CompRefuelable comp = this.GetComp<CompRefuelable>();
				if ((comp != null && comp.HasFuel) || comp == null)
				{
					FleckMaker.Static(this.Position, this.Map, LunaBRFDefOf.BlastEMP);
				}
			}
		}
	}
}
