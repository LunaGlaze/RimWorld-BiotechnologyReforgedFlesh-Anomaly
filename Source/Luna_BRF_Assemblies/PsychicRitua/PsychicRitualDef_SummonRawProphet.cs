using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualDef_SummonRawProphet : PsychicRitualDef_InvocationCircle
    {

        public SimpleCurve shockDurationFromQualityCurve;

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
            list.Add(new PsychicRitualToil_SummonRawProphet(InvokerRole, TargetRole));
            return list;
        }
        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.Abs(shockDurationFromQualityCurve.Evaluate(qualityRange.min)).ToString());
        }
    }
}