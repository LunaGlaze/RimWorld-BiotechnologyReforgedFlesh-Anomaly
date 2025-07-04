using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_DeathRefusalGiver : HediffComp
    {
        public HediffCompProperties_DeathRefusalGiver Props => (HediffCompProperties_DeathRefusalGiver)props;
        public int nextCheckTick;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (this.nextCheckTick == 0)
            {
                this.nextCheckTick = this.NextCheckTick;
            }
            if (Find.TickManager.TicksGame >= this.nextCheckTick)
            {
                this.nextCheckTick = this.NextCheckTick;
                Hediff DeathRefusal = Pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.DeathRefusal);
                Hediff DeathRefusalCreepJoiner = Pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("DeathRefusalCreepJoiner"));
                Hediff DeathRefusalSickness = Pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDefOf.DeathRefusalSickness);
                if (DeathRefusal == null && DeathRefusalCreepJoiner == null && DeathRefusalSickness == null)
                {
                    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DeathRefusal, Pawn);
                    Pawn.health.AddHediff(hediff);
                }
            }
        }
        public int NextCheckTick
        {
            get
            {
                return Find.TickManager.TicksGame + this.Props.checkTickRate.RandomInRange;
            }
        }
    }
}
