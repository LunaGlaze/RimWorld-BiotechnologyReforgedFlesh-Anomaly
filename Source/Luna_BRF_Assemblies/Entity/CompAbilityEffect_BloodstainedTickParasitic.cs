using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompAbilityEffect_BloodstainedTickParasitic : CompAbilityEffect
    {
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if(target.Pawn != null)
			{
				if (!target.Pawn.Dead)
				{
					Log.Error("BloodstainedTick tried to parasitic a pawn who is not dead: " + target.Pawn.ToStringSafe());
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_BloodstainedTickParasitic).ResetCooldown();
					return;
				}
				if (target.Pawn.Discarded)
				{
					Log.Error("BloodstainedTick tried to parasitic a discarded pawn: " + target.Pawn.ToStringSafe());
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_BloodstainedTickParasitic).ResetCooldown();
					return;
				}
			}
			if(target.Thing != null)
			{
				if (target.Thing is Corpse corpse)
				{
					if (!corpse.InnerPawn.RaceProps.IsFlesh || corpse.InnerPawn.RaceProps.FleshType != FleshTypeDefOf.Insectoid)
					{
						parent.pawn.abilities.GetAbility(LunaDefOf.BRF_BloodstainedTickParasitic).ResetCooldown();
						return;
					}
					else
					{
						Hediff hediff = HediffMaker.MakeHediff(LunaDefOf.BRF_BloodstainedTickParasiticed, corpse.InnerPawn);
						corpse.InnerPawn.health.AddHediff(hediff);
						if (corpse.InnerPawn.Faction != Faction.OfEntities)
						{
							corpse.InnerPawn.SetFaction(Faction.OfEntities);
						}
						ResurrectionUtility.TryResurrect(corpse.InnerPawn, new ResurrectionParams
						{
							gettingScarsChance = 0.1f,
							canKidnap = false,
							canTimeoutOrFlee = false,
							useAvoidGridSmart = true,
							canSteal = false,
							invisibleStun = true
						});
						parent.pawn.Destroy();
					}
				}
				else
				{
					Log.Error("BloodstainedTick tried to parasitic a target is not corpse: " + target.Thing.ToStringSafe());
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_BloodstainedTickParasitic).ResetCooldown();
					return;
				}
			}
		}
	}
}