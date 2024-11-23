using System;
using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class Luna_HediffAddBullet : Bullet
    {
        public ExtensionDef_LunaHediffAddBullet Props => def.GetModExtension<ExtensionDef_LunaHediffAddBullet>();
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            base.Impact(hitThing, blockedByShield);
            if (Props != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                float rand = Rand.Value;
                if (rand <= Props.addHediffChance)
                {
                    if (Props.HediffDefTransform != null)
                    {
                        Hediff HediffDefTransform = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.HediffDefTransform);
                        if (Props.NeedHediffDef != null)
                        {
                            Hediff NeedHediffOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.NeedHediffDef);
                            if (NeedHediffOnPawn != null)
                            {
                                AddHediffTransform(hitPawn, HediffDefTransform);
                            }
                            else
                            {
                                AddHediff(hitPawn, Props.HediffDefTransform);
                            }
                        }
                        else
                        {
                            AddHediffTransform(hitPawn, HediffDefTransform);
                        }
                    }
                    else
                    {
                        if (Props.NeedHediffDef != null)
                        {
                            Hediff NeedHediffOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.NeedHediffDef);
                            if (NeedHediffOnPawn != null)
                            {
                                AddHediff(hitPawn, Props.HediffDefToAdd);
                            }
                        }
                        else
                        {
                            AddHediff(hitPawn, Props.HediffDefToAdd);
                        }
                    }
                }
            }
        }
        public void AddHediffTransform(Pawn pawn , Hediff transHediff)
        {
            if (transHediff != null)
            {
                float severity = transHediff.Severity;
                pawn.health.RemoveHediff(transHediff);
                Hediff TargetHediffOnPawn = pawn.health?.hediffSet?.GetFirstHediffOfDef(Props.HediffDefToAdd);
                if (TargetHediffOnPawn != null)
                {
                    TargetHediffOnPawn.Severity = TargetHediffOnPawn.Severity + severity;
                }
                else
                {
                    Hediff hediff = HediffMaker.MakeHediff(Props.HediffDefToAdd, pawn);
                    hediff.Severity = severity;
                    pawn.health.AddHediff(hediff);
                }
            }
            else
            {
                AddHediff(pawn, Props.HediffDefToAdd);
            }
        }
        public void AddHediff (Pawn pawn , HediffDef hediffDef)
        {
            Hediff TargetHediffOnPawn = pawn.health?.hediffSet?.GetFirstHediffOfDef(hediffDef);
            float randomSeverity = Rand.Range(0.15f, 0.30f);
            if (TargetHediffOnPawn != null)
            {
                TargetHediffOnPawn.Severity += randomSeverity;
            }
            else
            {
                Hediff hediff = HediffMaker.MakeHediff(hediffDef, pawn);
                hediff.Severity = randomSeverity;
                pawn.health.AddHediff(hediff);
            }
        }
    }
}
