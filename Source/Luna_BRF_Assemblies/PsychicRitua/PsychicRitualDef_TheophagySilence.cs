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
    public class PsychicRitualDef_TheophagySilence : PsychicRitualDef_InvocationCircle
    {
        public SimpleCurve psychicShockChanceFromQualityCurve;
        public FloatRange darkPsychicShockDurarionHoursRange;
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TheophagySilence(InvokerRole, ChanterRole));
            return list;
        }
        public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
        {
            return outcomeDescription.Formatted(Mathf.Abs(psychicShockChanceFromQualityCurve.Evaluate(qualityRange.min)).ToString());
        }
    }
}
