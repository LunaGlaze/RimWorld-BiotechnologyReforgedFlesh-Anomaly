using System.Collections.Generic;
using Verse;

namespace Luna_BRF
{
    public class Hediff_Akuloth : Hediff_Implant
    {
        private static readonly FloatRange floatRangeA = new FloatRange(0.6f, 1);
        public override void Tick()
        {
            base.Tick();
            if (pawn.IsHashIntervalTick(6000))
            {
                InfectionResolves();
            }
        }
        public void InfectionResolves()
        {
            List<Hediff> hediffs = new List<Hediff>();
            pawn.health.hediffSet.GetHediffs(ref hediffs);
            foreach (Hediff buff in hediffs)
            {
                if (buff.def.isBad && buff != this && (buff.def.isInfection || buff.CanEverKill() || buff.canBeThreateningToPart) )
                {
                    float radomFloat = floatRangeA.RandomInRange;
                    if (Rand.Bool && Rand.Bool)
                    {
                        buff.Severity *= radomFloat;
                    }
                    if(buff.def.lethalSeverity > 0 && buff.Severity > (buff.def.lethalSeverity * (0.5f + (radomFloat / 2.5f))) && Rand.Bool)
                    {
                        buff.Severity = buff.def.lethalSeverity * (0.5f + (radomFloat / 2.5f));
                    }
                }
            }
        }
    }
}
