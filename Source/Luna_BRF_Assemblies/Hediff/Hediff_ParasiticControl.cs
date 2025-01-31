using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class Hediff_ParasiticControl : HediffWithComps
	{
		public Pawn parasite;
		public ExtensionDef_ParasiticControlConfiguration defExtension => this.def.GetModExtension<ExtensionDef_ParasiticControlConfiguration>();
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look(ref parasite, "BRF_ParasiticControl_parasite", true);
		}
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			this.parasitismEnds();
		}
        public override void Notify_Downed()
		{
			base.Notify_Downed();
			if (defExtension.parasitismEndsWhenDowned)
			{
				this.parasitismEnds();
			}
        }
        public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Pawn pawn = this.pawn;
			if (defExtension != null)
            {
                if (!defExtension.giveHediffDefsAlso.NullOrEmpty())
				{
					foreach (StartingHediff startingHediff in defExtension.giveHediffDefsAlso)
					{
						if (!startingHediff.HasHediff(pawn) && Rand.Chance(startingHediff.chance ?? 1f))
						{
							Hediff hediff = pawn.health.AddHediff(startingHediff.def);
							if (startingHediff.severity.HasValue)
							{
								hediff.Severity = startingHediff.severity.Value;
							}
							if (startingHediff.durationTicksRange.HasValue)
							{
								hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = startingHediff.durationTicksRange.Value.RandomInRange;
							}
						}
					}
                }
                if (!defExtension.removeHediffDefsAlso.NullOrEmpty())
                {
                    foreach (HediffDef hediff in defExtension.removeHediffDefsAlso)
                    {
                        Hediff hediff2 = pawn.health?.hediffSet?.GetFirstHediffOfDef(hediff);
                        if (hediff2 != null)
                        {
                            pawn.health.RemoveHediff(hediff2);
                        }
                    }
                }
            }
        }
        public override void PostTick()
		{
			base.PostTick();
			Pawn pawn = this.pawn;
			if (this.pawn.IsHashIntervalTick(512) && defExtension != null)
			{
				if (!defExtension.giveHediffDefsAlso.NullOrEmpty())
				{
					foreach (StartingHediff startingHediff in defExtension.giveHediffDefsAlso)
					{
						if (!startingHediff.HasHediff(pawn) && Rand.Chance(startingHediff.chance ?? 1f))
						{
							Hediff hediff = pawn.health.AddHediff(startingHediff.def);
							if (startingHediff.severity.HasValue)
							{
								hediff.Severity = startingHediff.severity.Value;
							}
							if (startingHediff.durationTicksRange.HasValue)
							{
								hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = startingHediff.durationTicksRange.Value.RandomInRange;
							}
						}
					}
				}
				if (!defExtension.removeHediffDefsAlso.NullOrEmpty())
				{
					foreach (HediffDef hediff in defExtension.removeHediffDefsAlso)
					{
						Hediff hediff2 = pawn.health?.hediffSet?.GetFirstHediffOfDef(hediff);
						if (hediff2 != null)
						{
							pawn.health.RemoveHediff(hediff2);
						}
					}
				}
			}
        }
		public void parasitismEnds()
		{
			Pawn pawn = this.pawn;
			if(parasite != null)
			{
				parasite.jobs.StopAll();
				GenSpawn.Spawn(parasite, pawn.Position, pawn.Map, WipeMode.VanishOrMoveAside);
			}
			if(defExtension != null)
            {
				if(ModsConfig.AnomalyActive && defExtension.codexEntry != null)
				{
					Find.EntityCodex.SetDiscovered(defExtension.codexEntry,parasite.def);
					if (parasite != null)
					{
						Find.HiddenItemsManager.SetDiscovered(parasite.def);
					}
				}
				if (!defExtension.endGiveHediffList.NullOrEmpty())
				{
					foreach (StartingHediff startingHediff in defExtension.endGiveHediffList)
					{
						if (!startingHediff.HasHediff(pawn) && Rand.Chance(startingHediff.chance ?? 1f))
						{
							Hediff hediff = pawn.health.AddHediff(startingHediff.def);
							if (startingHediff.severity.HasValue)
							{
								hediff.Severity = startingHediff.severity.Value;
							}
							if (startingHediff.durationTicksRange.HasValue)
							{
								hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = startingHediff.durationTicksRange.Value.RandomInRange;
							}
						}
					}
				}
				if (!defExtension.endGiveHediffListMalignant.NullOrEmpty() && pawn.health?.hediffSet?.GetFirstHediffOfDef(HediffDef.Named("BRF_MalignantFleshActivation")) != null)
				{
					foreach (StartingHediff startingHediff in defExtension.endGiveHediffListMalignant)
					{
						if (!startingHediff.HasHediff(pawn) && Rand.Chance(startingHediff.chance ?? 1f))
						{
							Hediff hediff = pawn.health.AddHediff(startingHediff.def);
							if (startingHediff.severity.HasValue)
							{
								hediff.Severity = startingHediff.severity.Value;
							}
							if (startingHediff.durationTicksRange.HasValue)
							{
								hediff.TryGetComp<HediffComp_Disappears>().ticksToDisappear = startingHediff.durationTicksRange.Value.RandomInRange;
							}
						}
					}
				}
				BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
				if (brain != null && defExtension.damageBrain)
				{
					float maxBrainHealth = brain.def.GetMaxHealth(pawn);
					float minHealthOfPartsWeWantToAvoidDestroying = GetMinHealthOfPartsWeWantToAvoidDestroying(brain, pawn);
					int num = Mathf.Min(GenMath.RoundRandom(maxBrainHealth * Rand.Range(0.25f, 0.5f)), GenMath.RoundRandom(minHealthOfPartsWeWantToAvoidDestroying * Rand.Range(0.25f, 0.75f)));
					if (num > 0)
					{
						DamageDef chosenDamageDef = DamageDefOf.Blunt;
						DamageInfo dinfo = new DamageInfo(chosenDamageDef, num, 0f, -1f, null, brain);
						dinfo.SetIgnoreArmor(ignoreArmor: true);
						dinfo.SetIgnoreInstantKillProtection(ignore: true);
						pawn.TakeDamage(dinfo);
					}
				}
				if (!defExtension.endRemoveHediffList.NullOrEmpty())
				{
					List<Hediff> hediffsToRemove = new List<Hediff>();
					foreach (HediffDef hediff in defExtension.endRemoveHediffList)
					{
						Hediff hediffToRemove = pawn.health?.hediffSet?.GetFirstHediffOfDef(hediff);
						if (hediffToRemove != null)
						{
							hediffsToRemove.Add(hediffToRemove);
						}
					}
					foreach (Hediff hediff in hediffsToRemove)
					{
						pawn.health.RemoveHediff(hediff);
					}
				}
			}
			pawn.health.RemoveHediff(this);
		}
		private static float GetMinHealthOfPartsWeWantToAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			float num = 999999f;
			while (part != null)
			{
				if (ShouldRandomDamageAvoidDestroying(part, pawn))
				{
					num = Mathf.Min(num, pawn.health.hediffSet.GetPartHealth(part));
				}
				part = part.parent;
			}
			return num;
		}
		private static bool ShouldRandomDamageAvoidDestroying(BodyPartRecord part, Pawn pawn)
		{
			if (part == pawn.RaceProps.body.corePart)
			{
				return true;
			}
			if (part.def.tags.Any((BodyPartTagDef x) => x.vital))
			{
				return true;
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				if (ShouldRandomDamageAvoidDestroying(part.parts[i], pawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
