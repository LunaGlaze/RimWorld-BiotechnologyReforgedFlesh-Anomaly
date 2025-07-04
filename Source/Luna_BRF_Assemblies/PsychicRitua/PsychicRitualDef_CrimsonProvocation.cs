using RimWorld;
using System.Collections.Generic;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualDef_CrimsonProvocation : PsychicRitualDef_VoidProvocation
    {
        public class EntityAssaultIncidentList
        {
            public IncidentDef incidentDef;
            public float selectionWeight = 1;
        }
        public List<EntityAssaultIncidentList> fleshEntityAssaultList = new List<EntityAssaultIncidentList>();

        public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
            list.Add(new PsychicRitualToil_CrimsonProvocation(InvokerRole, psychicShockChanceFromQualityCurve));
            return list;
        }
    }
}
