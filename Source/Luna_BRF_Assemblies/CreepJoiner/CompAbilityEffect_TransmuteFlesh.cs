using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class CompAbilityEffect_TransmuteFlesh : CompAbilityEffect
    {
        public new CompProperties_TransmuteFlesh Props => (CompProperties_TransmuteFlesh)props;
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Thing thing = target.Thing;
			TryGetElementFor(thing.def, out var ratio);
			int stackCount = thing.stackCount;
			int num = Mathf.Max(Mathf.FloorToInt((float)thing.stackCount * ratio.ratio), 1);
			ThingDef outputThingDef = null;
			Thing thingOutput = null;
			if (Props.specialTransmuteList != null)
            {
				outputThingDef = TryGetSpecialOutputFor(thing.def, out var specialTransmute);
			}
			if(outputThingDef != null)
            {
				thingOutput = ThingMaker.MakeThing(outputThingDef);
			}
            else
            {
				thingOutput = ThingMaker.MakeThing(Props.outcomeItems.RandomElement());
			}
			thingOutput.stackCount = num;
			IntVec3 positionHeld = thing.PositionHeld;
			thing.Destroy();
			GenPlace.TryPlaceThing(thingOutput, positionHeld, parent.pawn.Map, ThingPlaceMode.Direct);
			string arg = $"{stackCount} {target.Thing.LabelNoCount}";
			string arg2 = $"{num} {thingOutput.LabelNoCount}";
			Messages.Message("MessageTransmutedItem".Translate(parent.pawn.Named("PAWN"), arg.Named("ORIGINAL"), arg2.Named("TRANSMUTED")), parent.pawn, MessageTypeDefOf.NeutralEvent);
		}
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			CompProperties_TransmuteFlesh.TFElementRatio ratio;
			if (target.HasThing)
			{
				return TryGetElementFor(target.Thing.def, out ratio);
			}
			return false;
		}
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			CompProperties_TransmuteFlesh.TFElementRatio ratio;
			bool flag = target.HasThing && this.TryGetElementFor(target.Thing.def, out ratio);
			if (!flag)
			{
				Messages.Message(this.Props.failedMessage.Formatted(target.Thing.Label), target.Thing, MessageTypeDefOf.NeutralEvent, true);
			}
			return flag;
		}
		private bool TryGetElementFor(ThingDef thing, out CompProperties_TransmuteFlesh.TFElementRatio ratio)
		{
            ratio = Props.elementRatios.FirstOrDefault((CompProperties_TransmuteFlesh.TFElementRatio x) => x.fixedSourceFilter.Allows(thing));
			if(ratio == null && thing.thingCategories?.Contains(ThingCategoryDef.Named("MeatRaw")) != false)
            {
				ratio = new CompProperties_TransmuteFlesh.TFElementRatio();
				ratio.ratio = 1f;
				ThingFilter thingFilter = ThingFilter.CreateOnlyEverStorableThingFilter();
				ratio.fixedSourceFilter = thingFilter;
			}
			return ratio != null;
		}
		private ThingDef TryGetSpecialOutputFor(ThingDef thing, out CompProperties_TransmuteFlesh.SpecialTransmuteList specialTransmute)
		{
			specialTransmute = Props.specialTransmuteList.FirstOrDefault((CompProperties_TransmuteFlesh.SpecialTransmuteList x) => x.specialInput == thing);
			return specialTransmute != null ? specialTransmute.specialOutput : null;
		}
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
