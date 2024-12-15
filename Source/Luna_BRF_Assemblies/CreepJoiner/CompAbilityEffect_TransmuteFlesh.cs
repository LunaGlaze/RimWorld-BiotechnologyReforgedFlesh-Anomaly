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
			int num;
			if (target.HasThing)
			{
				num = (TryGetElementFor(target.Thing.def, out var _) ? 1 : 0);
				if (num != 0)
				{
					goto IL_0070;
				}
			}
			else
			{
				num = 0;
			}
			Messages.Message(Props.failedMessage.Formatted(target.Thing.Label), target.Thing, MessageTypeDefOf.NeutralEvent);
			goto IL_0070;
			IL_0070:
			return (byte)num != 0;
		}
		private bool TryGetElementFor(ThingDef stuff, out CompProperties_TransmuteFlesh.TFElementRatio ratio)
		{
            ratio = Props.elementRatios.FirstOrDefault((CompProperties_TransmuteFlesh.TFElementRatio x) => x.fixedSourceFilter.Allows(stuff));
			return ratio != null;
		}
		private ThingDef TryGetSpecialOutputFor(ThingDef stuff, out CompProperties_TransmuteFlesh.SpecialTransmuteList specialTransmute)
		{
			specialTransmute = Props.specialTransmuteList.FirstOrDefault((CompProperties_TransmuteFlesh.SpecialTransmuteList x) => x.specialInput == stuff);
			return specialTransmute.specialInput;
		}
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			return false;
		}
	}
}
