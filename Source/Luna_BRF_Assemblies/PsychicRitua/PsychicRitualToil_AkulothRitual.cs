using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_AkulothRitual : PsychicRitualToil
    {
        public PsychicRitualRoleDef targetRole;
        public PsychicRitualRoleDef invokerRole;
        protected PsychicRitualToil_AkulothRitual()
        {
        }
        public PsychicRitualToil_AkulothRitual(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
        {
            target.Strip(true);
            target.Kill(null, null);
            target.Corpse.Destroy();
            Hediff akuloth = HediffMaker.MakeHediff(LunaDefOf.BRF_Akuloth, invoker);
            invoker.health.AddHediff(akuloth);
            TaggedString text = "BRF_RitualSacrificedFinishText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL")) + "\n\n" + "BRF_AkulothRitualFinishText".Translate(invoker.Named("INVOKER"));
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Values.Look(ref targetRole, "targetRole");
        }
    }
}
