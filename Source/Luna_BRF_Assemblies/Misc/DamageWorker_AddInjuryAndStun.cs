using Verse;

namespace Luna_BRF
{
    public class DamageWorker_AddInjuryAndStun : DamageWorker_AddInjury
	{
		public override DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
