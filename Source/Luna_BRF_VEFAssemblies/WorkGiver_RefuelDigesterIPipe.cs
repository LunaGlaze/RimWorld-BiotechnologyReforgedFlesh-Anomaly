using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF_VEFAssemblies
{
	public class WorkGiver_RefuelDigesterIPipe : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForGroup(ThingRequestGroup.Refuelable);

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
