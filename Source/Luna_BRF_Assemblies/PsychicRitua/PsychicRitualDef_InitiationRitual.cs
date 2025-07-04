using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI.Group;
using Verse;
using UnityEngine;

namespace Luna_BRF
{
    public class PsychicRitualDef_InitiationRitual : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve skillOffsetPercentFromQualityCurve;
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_InitiationRitual(InvokerRole, TargetRole));
            list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
            return list;
        }

        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.Abs(skillOffsetPercentFromQualityCurve.Evaluate(qualityRange.min)).ToStringPercent());
        }
    }
}
