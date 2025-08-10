using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualDef_TheophagyKnowledge : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve basicKnowledgePointsFromQualityCurve;
        public SimpleCurve psychicShockChanceFromQualityCurve;
        public FloatRange darkPsychicShockDurarionHoursRange;
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TheophagyKnowledge(InvokerRole, ChanterRole, basicKnowledgePointsFromQualityCurve));
            return list;
        }
        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.Abs(basicKnowledgePointsFromQualityCurve.Evaluate(qualityRange.min)).ToString());
        }
    }
}
