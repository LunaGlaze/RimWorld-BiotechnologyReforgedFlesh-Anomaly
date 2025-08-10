using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public class WorkGiver_Equip_PsycorpseGuard : WorkGiver_Scanner
    {
        public override ThingRequest PotentialWorkThingRequest => ThingRequest.ForDef(LunaBRFDefOf.BRF_PsycorpseGuard);
        public override PathEndMode PathEndMode => PathEndMode.ClosestTouch;

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is ShooterGuardWithWeapon shooterGuardWithWeapon) || !shooterGuardWithWeapon.Spawned || shooterGuardWithWeapon.ReservedWeapon == null || !shooterGuardWithWeapon.ReservedWeapon.Spawned)
            {
                return false;
            }
            if (pawn.Faction != shooterGuardWithWeapon.Faction)
            {
                return false;
            }
            if (shooterGuardWithWeapon.IsForbidden(pawn) || shooterGuardWithWeapon.ReservedWeapon.IsForbidden(pawn))
            {
                return false;
            }
            if (!pawn.CanReserve(shooterGuardWithWeapon, 1, -1, null, forced) || !pawn.CanReserve(shooterGuardWithWeapon.ReservedWeapon, 1, -1, null, forced))
            {
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            if (!(t is ShooterGuardWithWeapon shooterGuardWithWeapon))
            {
                return null;
            }
            Thing reservedWeapon = shooterGuardWithWeapon.ReservedWeapon;
            if (reservedWeapon == null)
            {
                return null;
            }
            if (!pawn.CanReach(shooterGuardWithWeapon, PathEndMode.Touch, MaxPathDanger(pawn)) || !pawn.CanReach(reservedWeapon, PathEndMode.ClosestTouch, MaxPathDanger(pawn)))
            {
                return null;
            }
            Job job = HaulAIUtility.HaulToContainerJob(pawn, reservedWeapon, t);
            job.count = 1;
            return job;
        }
    }
}
