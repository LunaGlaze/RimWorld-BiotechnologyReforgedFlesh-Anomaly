using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_SummonClumps : PsychicRitualToil
    {
        private PsychicRitualRoleDef invokerRole;

        private FloatRange combatPointMultiplierFromQualityRange;

        private static readonly IntRange AnimalsDelayTicks = new IntRange(2500, 7500);
        protected PsychicRitualToil_SummonClumps()
        {
        }
        public PsychicRitualToil_SummonClumps(PsychicRitualRoleDef invokerRole, FloatRange combatPointMultiplierFromQualityRange)
        {
            this.invokerRole = invokerRole;
            this.combatPointMultiplierFromQualityRange = combatPointMultiplierFromQualityRange;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            base.Start(psychicRitual, parent);
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            float combatPointMultiplier = combatPointMultiplierFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
            psychicRitual.ReleaseAllPawnsAndBuildings();
            if (pawn != null)
            {
                ApplyOutcome(psychicRitual, pawn, combatPointMultiplier);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float combatPointMultiplier)
        {
            IncidentParms incidentParms = new IncidentParms();
            incidentParms.target = invoker.Map;
            incidentParms.pointMultiplier = combatPointMultiplier;
            incidentParms.forced = true;
            Find.Storyteller.incidentQueue.Add(LunaBRFDefOf.BRF_ClumpAssault, Find.TickManager.TicksGame + AnimalsDelayTicks.RandomInRange, incidentParms);
            LetterDef textLetterDef = LetterDefOf.NeutralEvent;
            TaggedString text = "BRF_SummonClumpsFinishText".Translate(invoker, psychicRitual.def.Named("RITUAL"));
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), text, textLetterDef);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Values.Look(ref combatPointMultiplierFromQualityRange, "combatPointMultiplierFromQualityRange");
        }
    }
}
