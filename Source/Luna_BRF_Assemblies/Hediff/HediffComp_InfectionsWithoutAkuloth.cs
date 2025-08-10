using RimWorld;
using UnityEngine;
using Verse;
using static HarmonyLib.Code;

namespace Luna_BRF
{
    public class HediffComp_InfectionsWithoutAkuloth : HediffComp
    {
        public int tickInterval = 2000;
        private static readonly FloatRange InfectionCount = new FloatRange(0, 6);

        public bool HasAkuloth(Pawn p)
        {
            return p.health.hediffSet.HasHediff(LunaBRFDefOf.BRF_Akuloth);
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
                Hediff WoundInfectionOnPawn = pawn.health?.GetOrAddHediff(HediffDefOf.WoundInfection, parent.Part);
                float point = InfectionCount.RandomInRange;
                WoundInfectionOnPawn.Severity = Mathf.Min(1, 0.01f * point + WoundInfectionOnPawn.Severity);
            }
        }
    }
}
