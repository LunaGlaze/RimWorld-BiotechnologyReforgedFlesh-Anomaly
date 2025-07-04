using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_InfectionsWithoutAkuloth : HediffComp
    {
        public int tickInterval = 2500;

        public bool HasAkuloth(Pawn p)
        {
            return p.health.hediffSet.HasHediff(LunaDefOf.BRF_Akuloth);
        }

        public override void CompPostMake()
        {
            Pawn pawn = parent.pawn;
            if (!HasAkuloth(pawn))
            {
                base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, parent.Part);
            }
            base.CompPostMake();
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (!parent.pawn.IsHashIntervalTick(tickInterval))
            {
                return;
            }
            Pawn pawn = parent.pawn;
            if (!HasAkuloth(pawn))
            {
                base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, parent.Part);
            }
        }
    }
}
