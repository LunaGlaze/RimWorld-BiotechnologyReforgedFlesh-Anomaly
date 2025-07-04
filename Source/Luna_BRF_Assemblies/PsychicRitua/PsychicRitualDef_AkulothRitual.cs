using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI.Group;
using Verse;

namespace Luna_BRF
{
    public class PsychicRitualDef_AkulothRitual : PsychicRitualDef_InvocationCircle
    {
        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_TargetCleanup(InvokerRole, TargetRole));
            list.Add(new PsychicRitualToil_AkulothRitual(InvokerRole, TargetRole));
            return list;
        }
    }
}
