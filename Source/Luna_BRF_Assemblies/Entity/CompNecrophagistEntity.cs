using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Luna_BRF
{
    class CompNecrophagistEntity : ThingComp, IThingHolder
    {
		public CompNecrophagistEntity()
		{
			innerContainer = new ThingOwner<Thing>(this);
		}
		private ThingOwner<Thing> innerContainer;
		private int ticksDigesting;
		public Pawn Pawn => parent as Pawn;
		public bool Digesting => DigestingThing != null;
		public Thing DigestingThing
		{
			get
			{
				if (innerContainer.InnerListForReading.Count <= 0)
				{
					return null;
				}
				return innerContainer.InnerListForReading[0];
			}
		}
		public Pawn DigestingPawn
		{
			get
			{
				Thing digestingThing = DigestingThing;
				if (digestingThing == null)
				{
					return null;
				}
				if (digestingThing is Corpse corpse)
				{
					return corpse.InnerPawn;
				}
				return digestingThing as Pawn;
			}
		}
		public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }
        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }
		public override void CompTick()
		{
			if (Digesting)
			{
				ticksDigesting++;
			}
			innerContainer.ThingOwnerTick(removeIfDestroyed: false);
			if (Digesting && DigestingPawn != null && DigestingPawn.Dead)
			{
				CompleteDigestion();
			}
		}
		public void StartDigesting(IntVec3 origin, LocalTargetInfo target)
		{
			if (target.Thing is Corpse corpse && corpse.InnerPawn.RaceProps.IsFlesh)
			{
				target.Thing.DeSpawn();
				ticksDigesting = 0;
				innerContainer.TryAdd(target.Thing);
                Pawn.jobs.StartJob(JobMaker.MakeJob(LunaDefOf.BRF_CorpseNecrophagistDigest), JobCondition.InterruptForced);
				Pawn.Rotation = Rot4.FromAngleFlat((parent.Position - origin).AngleFlat);
				if (Pawn.Drawer.renderer.CurAnimation != AnimationDefOf.DevourerDigesting)
				{
					Pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DevourerDigesting);
				}
			}
			else if (target.HasThing && target.Thing is Pawn pawn && pawn.Spawned && pawn.RaceProps.IsFlesh)
			{
				pawn.DeSpawn();
				ticksDigesting = 0;
				innerContainer.TryAdd(pawn);
				Pawn.jobs.StartJob(JobMaker.MakeJob(LunaDefOf.BRF_CorpseNecrophagistDigest), JobCondition.InterruptForced);
				Pawn.Rotation = Rot4.FromAngleFlat((parent.Position - origin).AngleFlat);
				if (Pawn.Drawer.renderer.CurAnimation != AnimationDefOf.DevourerDigesting)
				{
					Pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DevourerDigesting);
				}
			}
            else
            {
				Pawn.abilities.GetAbility(LunaDefOf.BRF_EntityNecrophagia).ResetCooldown();
				innerContainer.TryDropAll(Pawn.PositionHeld, Pawn.MapHeld, ThingPlaceMode.Direct);
				if(target.Thing is Corpse)
                {
					target.Thing.Destroy();
				}else if(target.HasThing && target.Thing is Pawn)
                {
					Pawn pawn1 = target.Thing as Pawn;
					if (pawn1.Spawned) { target.Thing.Kill(); }
				}
			}
		}
		public int GetDigestionTicks()
		{
			if (DigestingThing == null)
			{
				return 0;
			}
			int num = Mathf.CeilToInt(5000f * DigestingPawn.BodySize / Pawn.BodySize);
			return num;
		}
		public void CompleteDigestion()
		{
			if (!Digesting)
			{
				return;
			}
			float bodySize = DigestingPawn.BodySize;
			bool isflesh = DigestingPawn.RaceProps.IsFlesh;
			if (DigestingThing is Pawn)
			{
				DigestingThing.Kill();
			}
			if (DigestingThing != null)
			{
				DigestingThing.Destroy();
			}
			Hediff firstHediff = Pawn.health?.hediffSet?.GetFirstHediffOfDef(LunaDefOf.BRF_RapidRegeneration);
			if (firstHediff != null && isflesh)
			{
				firstHediff.Severity += (bodySize/50) ;
            }else if (isflesh)
            {
				Hediff hediff = HediffMaker.MakeHediff(LunaDefOf.BRF_RapidRegeneration, Pawn);
				hediff.Severity = bodySize/50;
				Pawn.health.AddHediff(hediff);
			}
			if (Pawn.Drawer.renderer.CurAnimation == AnimationDefOf.DevourerDigesting)
			{
				Pawn.Drawer.renderer.SetAnimation(null);
			}
		}
		public override void Notify_Downed()
		{
			base.Notify_Downed();
			innerContainer.TryDropAll(Pawn.PositionHeld, Pawn.MapHeld, ThingPlaceMode.Direct);
			Pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
		}
		public override void Notify_Killed(Map prevMap, DamageInfo? _ = null)
		{
			base.Notify_Killed(prevMap, _);
			innerContainer.TryDropAll(Pawn.PositionHeld, Pawn.MapHeld, ThingPlaceMode.Direct);
		}
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref ticksDigesting, "ticksDigesting", 0);
			Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		}
		public override string CompInspectStringExtra()
		{
			string baseString = base.CompInspectStringExtra();
			if (Digesting)
			{
				Thing digestingThing = DigestingThing;
				if (digestingThing != null)
				{
					string contentName = digestingThing.LabelCap;
					return $"{baseString}\n {"NecrophagistEntityWhenDigestinng".Translate()} : {contentName}";
				}
			}
			return baseString;
		}

	}
}
