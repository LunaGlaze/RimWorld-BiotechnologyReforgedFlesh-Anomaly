using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualDef_SummonSwarms : PsychicRitualDef_InvocationCircle
    {
        private FloatRange combatPointMultiplierFromQualityRange;

		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph graph)
		{
			List<PsychicRitualToil> list = base.CreateToils(psychicRitual, graph);
			list.Add(new PsychicRitualToil_SummonSwarms(InvokerRole, combatPointMultiplierFromQualityRange));
			return list;
		}
	}
}