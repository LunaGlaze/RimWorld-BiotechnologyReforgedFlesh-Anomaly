using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI.Group;
using Verse;
using UnityEngine;

namespace Luna_BRF
{
    public class PsychicRitualToil_InitiationRitual : PsychicRitualToil
    {
        public PsychicRitualRoleDef targetRole;
        public PsychicRitualRoleDef invokerRole;
        protected PsychicRitualToil_InitiationRitual()
        {
        }
        public PsychicRitualToil_InitiationRitual(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            float skillOffsetPct = ((PsychicRitualDef_InitiationRitual)psychicRitual.def).skillOffsetPercentFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2, skillOffsetPct);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target, float skillOffsetPct)
        {
            Trait Orin = new Trait(LunaDefOf.BRF_Sarkicism, 0, true);
            target.story.traits.GainTrait(Orin);
            if (invoker.story.traits.HasTrait(LunaDefOf.BRF_Sarkicism))
            {
                TaggedString text = "BRF_InitiationRitualBaseFinishText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL"));
                Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
            }
            else
            {
                float num = 0f;
                foreach (SkillRecord skill in target.skills.skills)
                {
                    float num2 = skill.XpTotalEarned * skillOffsetPct;
                    skill.Learn(num2);
                    num += num2;
                }
                if (PawnUtility.ShouldSendNotificationAbout(target))
                {
                    Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "BRF_InitiationRitualCompleteText".Translate(target, Mathf.Abs((int)num)), LetterDefOf.NeutralEvent, target);
                }
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Values.Look(ref targetRole, "targetRole");
        }
    }
}
