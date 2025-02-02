using RimWorld;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    public abstract class JobGiver_AICastAbilityWithBashDoors : JobGiver_AICastAbility
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.CurJob?.ability?.def == this.ability)
			{
				return null;
			}
			Ability ability = pawn.abilities?.GetAbility(this.ability);
			if (ability == null || !ability.CanCast)
			{
				return null;
			}
			LocalTargetInfo target = GetTarget(pawn, ability);
			if (!target.IsValid)
			{
				return null;
			}
			Job job = ability.GetJob(target, target);
			job.canBashDoors = true;
			job.canBashFences = true;
			return job;
		}
	}
}
