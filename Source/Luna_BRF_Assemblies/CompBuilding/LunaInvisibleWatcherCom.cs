using RimWorld;
using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    public class LunaInvisibleWatcherCom : ThingComp
    {
        public CompProperties_InvisibleWatcher Props => (CompProperties_InvisibleWatcher)props;
		public float Range => Props.range;
		public override void CompTick()
		{
			base.CompTick();
			if (parent.Fogged() || !parent.Spawned || !parent.IsHashIntervalTick(Props.checkIntervalTicks) || (Props.requiresPower && !PowerOn) || (Props.requiresFuel && !HasFuel))
			{
				return;
			}
			IReadOnlyList<Pawn> allPawnsSpawned = parent.Map.mapPawns.AllPawnsSpawned;
			Faction selfFaction = parent.Faction;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].Faction != selfFaction && allPawnsSpawned[i].Position.InHorDistOf(parent.Position, Range))
				{
					Hediff firstHediffOfDef = allPawnsSpawned[i].health.hediffSet.GetFirstHediffOfDef(LunaBRFDefOf.BRF_InvisibleWatched);
					if (firstHediffOfDef == null && allPawnsSpawned[i].IsPsychologicallyInvisible())
					{
						firstHediffOfDef = allPawnsSpawned[i].health.AddHediff(LunaBRFDefOf.BRF_InvisibleWatched);
						allPawnsSpawned[i].Notify_BecameVisible();
						break;
					}
					HediffComp_Disappears hediffComp_Disappears = firstHediffOfDef?.TryGetComp<HediffComp_Disappears>();
					if (hediffComp_Disappears != null)
					{
						hediffComp_Disappears.ticksToDisappear = 2500;
					}
				}
			}
		}
		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}
		public bool HasFuel
		{
			get
			{
				CompRefuelable comp = this.parent.GetComp<CompRefuelable>();
				return comp != null && comp.HasFuel;
			}
		}
	}
}
