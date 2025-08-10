using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_AddHediffPostRemoved : HediffComp
    {
        public HediffCompProperties_AddHediffPostRemoved Props => (HediffCompProperties_AddHediffPostRemoved)props;
        public override void CompPostPostRemoved()
        {
            if(Props.hediffDef != null)
            {
                if (Props.allowDeath || !parent.pawn.Dead)
                {
                    if(Props.allowDown || !parent.pawn.Downed)
                    {
                        Hediff hediff = HediffMaker.MakeHediff(Props.hediffDef, parent.pawn);
                        parent.pawn.health.AddHediff(hediff);
                    }
                }
            }
        }
    }
}
