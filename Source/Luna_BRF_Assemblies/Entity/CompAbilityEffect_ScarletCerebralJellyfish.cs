using RimWorld;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class CompAbilityEffect_ScarletCerebralJellyfish : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (target.Pawn != null)
			{
				if (target.Pawn.Dead)
				{
					Log.Error("ScarletCerebralJellyfish tried to rape a pawn who is dead: " + target.Pawn.ToStringSafe());
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_ScarletCerebralJellyfishBrainInsertion).ResetCooldown();
					return;
				}
				if (target.Pawn.Discarded)
				{
					Log.Error("ScarletCerebralJellyfish tried to rape a discarded pawn: " + target.Pawn.ToStringSafe());
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_ScarletCerebralJellyfishBrainInsertion).ResetCooldown();
					return;
				}
				if (target.Pawn.RaceProps.IsMechanoid)
				{
					DamageDef chosenDamageDef = DamageDefOf.Blunt;
					DamageInfo dinfo = new DamageInfo(chosenDamageDef, 1, 0f, -1f);
					dinfo.SetIgnoreArmor(ignoreArmor: true);
					dinfo.SetIgnoreInstantKillProtection(ignore: true);
					BodyPartRecord brain = target.Pawn.health.hediffSet.GetBrain();
					if (brain != null)
					{
						float maxBrainHealth = brain.def.GetMaxHealth(target.Pawn);
						float minHealthOfPartsWeWantToAvoidDestroying = LunaBRFHediffUtility.GetMinHealthOfPartsWeWantToAvoidDestroying(brain, target.Pawn);
						int num = Mathf.Min(GenMath.RoundRandom(maxBrainHealth * Rand.Range(0.4f, 0.6f)), GenMath.RoundRandom(minHealthOfPartsWeWantToAvoidDestroying * Rand.Range(0.75f, 0.9f)));
						if (num > 0)
						{
							dinfo = new DamageInfo(chosenDamageDef, num, 0f, -1f, null, brain);
							dinfo.SetIgnoreArmor(ignoreArmor: true);
							dinfo.SetIgnoreInstantKillProtection(ignore: true);
							target.Pawn.TakeDamage(dinfo);
						}
					}
					target.Pawn.Kill(dinfo, null);
					parent.pawn.abilities.GetAbility(LunaDefOf.BRF_ScarletCerebralJellyfishBrainInsertion).ResetCooldown();
					return;
				}
				Pawn self = parent.pawn;
				Hediff_ParasiticControl hediff = (Hediff_ParasiticControl)HediffMaker.MakeHediff(HediffDef.Named("BRF_ScarletCerebralJellyfishBrainInsertion"), target.Pawn);
				self.health.Notify_Resurrected(true, 0f);
				hediff.parasite = self;
				self.DeSpawn();
				target.Pawn.health.AddHediff(hediff);
				Find.HiddenItemsManager.SetDiscovered(ThingDef.Named("BRF_ScarletCerebralJellyfish"));
			}
            else
			{
				Log.Error("ScarletCerebralJellyfish tried to rape a target which is not pawn: " + target.Thing.ToStringSafe());
				parent.pawn.abilities.GetAbility(LunaDefOf.BRF_ScarletCerebralJellyfishBrainInsertion).ResetCooldown();
				return;
			}
		}
    }
}
