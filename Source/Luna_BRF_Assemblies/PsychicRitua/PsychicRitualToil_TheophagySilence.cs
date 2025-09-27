using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_TheophagySilence : PsychicRitualToil
    {
        public PsychicRitualRoleDef chanterRole;
        public PsychicRitualRoleDef invokerRole;
        public PsychicRitualToil_TheophagySilence(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef chanterRole)
        {
            this.invokerRole = invokerRole;
            this.chanterRole = chanterRole;
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
            PsychicRitualDef_TheophagySilence psychicRitualDef_TheophagySilence = (PsychicRitualDef_TheophagySilence)psychicRitual.def;
            chanters.Add(invoker);
            if (!chanters.NullOrEmpty())
            {
                foreach (Pawn chanter in chanters)
                {
                    LunaBRFBaseUtility.AddFoodNeedPoints(chanter, 0.35f);
                }
            }
            Map map = psychicRitual.Map;
            List<Pawn> shambler = new List<Pawn>();
            foreach (Pawn item in map.mapPawns.AllPawnsSpawned)
            {
                if (item.IsShambler)
                {
                    shambler.Add(item);
                }
            }
            if (shambler.Count > 0)
            {
                foreach (Pawn item in shambler) {
                    item.Kill(null);
                }
            }
            //虚空扰动同款结算
            TaggedString text = "BRF_RitualTheophagy_RitualFinishText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + "BRF_TheophagyKnockSilenceFinishText".Translate();
            if (Rand.Chance(psychicRitualDef_TheophagySilence.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
            {
                text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                int duration = Mathf.RoundToInt(psychicRitualDef_TheophagySilence.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                invoker.health.AddHediff(hediff);
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
    }
}
