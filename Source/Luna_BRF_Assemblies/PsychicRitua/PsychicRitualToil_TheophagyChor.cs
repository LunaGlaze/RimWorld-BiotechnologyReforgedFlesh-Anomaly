using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_TheophagyChor : PsychicRitualToil
    {
        public PsychicRitualRoleDef chanterRole;
        public PsychicRitualRoleDef invokerRole;
        public SimpleCurve extraHealthPointsFromQualityCurve;
        public PsychicRitualToil_TheophagyChor(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef chanterRole, SimpleCurve extraHealthPointsFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.chanterRole = chanterRole;
            this.extraHealthPointsFromQualityCurve = extraHealthPointsFromQualityCurve;
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
            PsychicRitualDef_TheophagyChor psychicRitualDef_TheophagyChor = (PsychicRitualDef_TheophagyChor)psychicRitual.def;
            chanters.Add(invoker);
            if (!chanters.NullOrEmpty())
            {
                foreach (Pawn chanter in chanters)
                {
                    LunaBRFBaseUtility.AddFoodNeedPoints(chanter, 0.5f);
                }
            }
            float hpPoint = psychicRitualDef_TheophagyChor.extraHealthPointsFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            if (invoker != null) {
                LunaBRFHediffUtility.AddPointRapidRegeneration(invoker, hpPoint);
            }
            TaggedString text = "BRF_RitualTheophagy_RitualFinishText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + "BRF_TheophagyChorFinishText".Translate();
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
    }
}
