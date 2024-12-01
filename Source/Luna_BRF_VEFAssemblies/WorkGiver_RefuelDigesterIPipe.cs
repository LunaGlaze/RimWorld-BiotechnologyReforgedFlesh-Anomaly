using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace Luna_BRF_VEFAssemblies
{
	public class WorkGiver_RefuelDigesterIPipe : WorkGiver_Scanner
	{
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.GetComponent<DigesterIPipes_MapComponent>().comps.Select((LunaComPipSpawnRefuelable x) => x.parent);
		}
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(LunaBRF_VEFDefof.BRF_DigesterIPipe);
		public override PathEndMode PathEndMode => PathEndMode.Touch;

		public virtual JobDef JobStandard => LunaBRF_VEFDefof.BRF_RefuelDigesterIPipe;

		public virtual JobDef JobAtomic => LunaBRF_VEFDefof.BRF_RefuelDigesterIPipe_Atomic;

		public virtual bool CanRefuelThing(Thing t)
		{
			return !(t is Building_Turret);
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			if (CanRefuelThing(t))
			{
				return RefuelWorkGiverUtilityDigesterIPipe.CanRefuel(pawn, t, forced);
			}
			return false;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return RefuelWorkGiverUtilityDigesterIPipe.RefuelJob(pawn, t, forced, JobStandard, JobAtomic);
		}
	}
}
