using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using static Luna_BRF.PsychicRitualDef_CrimsonProvocation;

namespace Luna_BRF
{
    public class PsychicRitualToil_CrimsonProvocation : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;

        private SimpleCurve psychicShockChanceFromQualityCurve;
        public PsychicRitualToil_CrimsonProvocation(PsychicRitualRoleDef invokerRole, SimpleCurve psychicShockChanceFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.psychicShockChanceFromQualityCurve = psychicShockChanceFromQualityCurve;
        }

        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            if (pawn != null)
            {
                ApplyOutcome(psychicRitual, pawn);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
        {
            PsychicRitualDef_CrimsonProvocation psychicRitualDef_CrimsonProvocation = (PsychicRitualDef_CrimsonProvocation)psychicRitual.def;
            Map map = psychicRitual.Map;
            IncidentDef incidentDef = psychicRitualDef_CrimsonProvocation.fleshEntityAssaultList.RandomElementByWeight((EntityAssaultIncidentList x) => x.selectionWeight).incidentDef;
            bool flag;
            if (incidentDef != null)
            {
                flag = true;
                IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, map);
                incidentParms.bypassStorytellerSettings = true;
                incidentParms.forced = true;
                incidentParms.target = map;
                Find.Storyteller.incidentQueue.Add(incidentDef, Find.TickManager.TicksGame + Mathf.RoundToInt(psychicRitualDef_CrimsonProvocation.incidentDelayHoursRange.RandomInRange * 2500f), incidentParms);
            }
            else
            {
                flag = false;
            }
            //虚空扰动同款结算
            TaggedString text = "BRF_CrimsonProvocationCompletedText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag ? "VoidProvocationSucceeded" : "VoidProvocationFailed").Translate();
            if (flag && Rand.Chance(psychicRitualDef_CrimsonProvocation.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
            {
                text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                int duration = Mathf.RoundToInt(psychicRitualDef_CrimsonProvocation.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                invoker.health.AddHediff(hediff);
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
            foreach (Pawn item2 in PawnsFinder.AllMaps_FreeColonistsSpawned)
            {
                if (item2.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDefOf.VoidCuriosity) > 0)
                {
                    item2.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.VoidCuriosity);
                    item2.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.VoidCuriositySatisfied);
                }
            }
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Deep.Look(ref psychicShockChanceFromQualityCurve, "psychicShockChanceFromQualityCurve");
        }
    }
}
