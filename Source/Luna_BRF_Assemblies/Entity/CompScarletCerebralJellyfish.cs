using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class CompScarletCerebralJellyfish : ThingComp
	{
		private const float BaseVisibleRadius = 14f;

		private const int UndetectedTimeout = 1200;

		private const int CheckDetectedIntervalTicks = 7;

		private const float FirstDetectedRadius = 30f;

		private const int RevealedLetterDelayTicks = 6;

		private const int AmbushCallMTBTicks = 600;

		[Unsaved(false)]
		private HediffComp_Invisibility invisibility;

		private int lastDetectedTick = -99999;

		private static float lastNotified = -99999f;

		private const float NotifyCooldownSeconds = 60f;

		private Pawn ScarletCerebralJellyfish => (Pawn)parent;

		private HediffComp_Invisibility Invisibility => invisibility ?? (invisibility = ScarletCerebralJellyfish.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility)?.TryGetComp<HediffComp_Invisibility>());

		public override void PostExposeData()
		{
			Scribe_Values.Look(ref lastDetectedTick, "lastDetectedTick", 0);
		}

		public override void CompTick()
		{
			base.CompTick();
			if (ScarletCerebralJellyfish.IsShambler)
			{
				return;
			}
			if (Invisibility == null)
			{
				ScarletCerebralJellyfish.health.AddHediff(HediffDefOf.HoraxianInvisibility);
			}
			if (!ScarletCerebralJellyfish.Spawned)
			{
				return;
			}
			if (ScarletCerebralJellyfish.IsHashIntervalTick(7))
			{
				if (Find.TickManager.TicksGame > lastDetectedTick + 1200)
				{
					CheckDetected();
				}
				if (Find.TickManager.TicksGame > lastDetectedTick + 1200)
				{
					Invisibility.BecomeInvisible();
				}
			}
			Lord lord = ScarletCerebralJellyfish.GetLord();
			if (lord != null && Rand.MTBEventOccurs(600f, 1f, 1f) && (ScarletCerebralJellyfish.CurJob?.def == JobDefOf.Wait || lord.LordJob is LordJob_EntitySwarm))
			{
				ScarletCerebralJellyfish.caller?.DoCall();
			}
		}

		private void CheckDetected()
		{
			foreach (Pawn item in ScarletCerebralJellyfish.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				if (PawnCanDetect(item))
				{
					if (!Invisibility.PsychologicallyVisible)
					{
						Invisibility.BecomeVisible();
					}
					lastDetectedTick = Find.TickManager.TicksGame;
				}
			}
		}

		private bool PawnCanDetect(Pawn pawn)
		{
			if (pawn.Faction == Faction.OfEntities || pawn.Downed || !pawn.Awake())
			{
				return false;
			}
			if (pawn.IsAnimal)
			{
				return false;
			}
			if (!ScarletCerebralJellyfish.Position.InHorDistOf(pawn.Position, GetPawnSightRadius(pawn, ScarletCerebralJellyfish)))
			{
				return false;
			}
			return GenSight.LineOfSightToThing(pawn.Position, ScarletCerebralJellyfish, parent.Map);
		}

		private static float GetPawnSightRadius(Pawn pawn, Pawn ScarletCerebralJellyfish)
		{
			float num = 14f;
			if (pawn.genes == null || pawn.genes.AffectedByDarkness)
			{
				float t = ScarletCerebralJellyfish.Map.glowGrid.GroundGlowAt(ScarletCerebralJellyfish.Position);
				num *= Mathf.Lerp(0.33f, 1f, t);
			}
			return num * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Sight);
		}

		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (!ScarletCerebralJellyfish.IsShambler)
			{
				Invisibility.BecomeVisible();
				lastDetectedTick = Find.TickManager.TicksGame;
			}
		}

		public override void Notify_BecameVisible()
		{
			Find.EntityCodex.SetDiscovered(LunaEntityCodexEntryDefOf.BRF_ScarletCerebralJellyfish, ThingDef.Named("BRF_ScarletCerebralJellyfish"));
			foreach (Pawn item in ScarletCerebralJellyfish.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				if (item.kindDef == PawnKindDef.Named("BRF_ScarletCerebralJellyfish") && item != ScarletCerebralJellyfish && item.Position.InHorDistOf(ScarletCerebralJellyfish.Position, 30f) && !item.IsPsychologicallyInvisible() && GenSight.LineOfSight(item.Position, ScarletCerebralJellyfish.Position, item.Map))
				{
					return;
				}
			}
			if (RealTime.LastRealTime > lastNotified + 60f)
			{
				Find.LetterStack.ReceiveLetter("BRF_LetterLabelScarletCerebralJellyfishRevealed".Translate(), "BRF_LetterScarletCerebralJellyfishRevealed".Translate(), LetterDefOf.ThreatBig, ScarletCerebralJellyfish, null, null, null, null, 6);
			}
			else
			{
				Messages.Message("BRF_MessageScarletCerebralJellyfishRevealed".Translate(), ScarletCerebralJellyfish, MessageTypeDefOf.ThreatBig);
			}
			lastNotified = RealTime.LastRealTime;
			lastDetectedTick = Find.TickManager.TicksGame;
		}
	}
}
