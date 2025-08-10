using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class Hediff_BRF_ScarletFever : HediffWithComps
    {
        public override void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result)
        {
            if (!result.hediffs.NullOrEmpty())
            {
                if(thing is Pawn pawn && !pawn.RaceProps.IsFlesh)
                {
                    float mutl = 1.2f;
                    if (pawn.RaceProps.IsMechanoid) { mutl = 1.5f; }
                    //dinfo.SetAmount(dinfo.Amount * mutl);
                    foreach (Hediff hediff in result.hediffs)
                    {
                        hediff.Severity *= mutl;
                    }
                }
            }
        }
    }
}
