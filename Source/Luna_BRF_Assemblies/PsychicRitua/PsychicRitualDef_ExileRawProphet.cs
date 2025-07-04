using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI.Group;
using Verse;

namespace Luna_BRF
{
    public class PsychicRitualDef_ExileRawProphet : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve psychicShockChanceFromQualityCurve;

        public FloatRange darkPsychicShockDurarionHoursRange;

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
            list.Add(new PsychicRitualToil_ExileRawProphet(InvokerRole, TargetRole, psychicShockChanceFromQualityCurve));
            return list;
        }

        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(psychicShockChanceFromQualityCurve.Evaluate(qualityRange.min).ToStringPercent());
        }
    }
}
