using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_TheophagySatisfaction : PsychicRitualToil
    {
        public PsychicRitualRoleDef chanterRole;
        public PsychicRitualRoleDef invokerRole;

        private SimpleCurve durationHoursFromQualityCurve;
        public PsychicRitualToil_TheophagySatisfaction(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef chanterRole, SimpleCurve durationHoursFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.chanterRole = chanterRole;
            this.durationHoursFromQualityCurve = durationHoursFromQualityCurve;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            List<Pawn> chanters = psychicRitual.assignments.AssignedPawns(chanterRole).ToList();
            if (pawn != null)
            {
                ApplyOutcome(psychicRitual, pawn, chanters);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, List<Pawn> chanters)
        {
            PsychicRitualDef_TheophagySatisfaction psychicRitualDef_TheophagySatisfaction = (PsychicRitualDef_TheophagySatisfaction)psychicRitual.def;
            chanters.Add(invoker);
            if (!chanters.NullOrEmpty())
            {
                foreach (Pawn chanter in chanters)
                {
                    LunaBRFBaseUtility.AddFoodNeedPoints(chanter, 0.5f);
                    Hediff thirst = chanter.health.hediffSet.GetFirstHediffOfDef(LunaBRFDefOf.BRF_ThirstAmpulla);
                    if(thirst != null) { chanter.health.RemoveHediff(thirst); }
                    Hediff satisfaction = chanter.health.hediffSet.GetFirstHediffOfDef(LunaBRFDefOf.BRF_SatisfactionAmpulla);
                    float hours = psychicRitualDef_TheophagySatisfaction.durationHoursFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
                    int ticks = (int)(hours * 2500);
                    if (satisfaction != null)
                    {
                        HediffComp_Disappears hediffCompDisappears = satisfaction.TryGetComp<HediffComp_Disappears>();
                        if(hediffCompDisappears != null)
                        {
                            hediffCompDisappears.ticksToDisappear = Math.Max(hediffCompDisappears.ticksToDisappear,ticks);
                        }
                    }
                    else
                    {
                        satisfaction = HediffMaker.MakeHediff(LunaBRFDefOf.BRF_SatisfactionAmpulla, chanter);
                        HediffComp_Disappears hediffCompDisappears = satisfaction.TryGetComp<HediffComp_Disappears>();
                        if (hediffCompDisappears != null)
                        {
                            hediffCompDisappears.ticksToDisappear = ticks;
                        }
                        chanter.health.AddHediff(satisfaction);
                    }
                }
            }
            TaggedString text = "BRF_RitualTheophagy_RitualFinishText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + "BRF_TheophagySatisfactionFinishText".Translate();
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
    }
}
