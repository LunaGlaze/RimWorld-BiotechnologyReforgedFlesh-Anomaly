using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualDef_TheophagyChor : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve extraHealthPointsFromQualityCurve;
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TheophagyChor(InvokerRole, ChanterRole, extraHealthPointsFromQualityCurve));
            return list;
        }
        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.Abs(extraHealthPointsFromQualityCurve.Evaluate(qualityRange.min)).ToString());
        }
    }
}
