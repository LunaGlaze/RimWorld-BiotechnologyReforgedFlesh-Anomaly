using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class PsychicRitualRoleDef_InitiationTarget : PsychicRitualRoleDef
    {
        protected override bool PawnCanDo(Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
        {
            if (!base.PawnCanDo(context, pawn, target, out reason))
            {
                return false;
            }
            if (pawn.story.traits.HasTrait(LunaBRFDefOf.BRF_Sarkicism))
            {
                return false;
            }
            return true;
        }
        public override TaggedString PawnCannotDoReason(AnyEnum reason, Context context, Pawn pawn, TargetInfo target)
        {
            return "BRF_psychicRitualLeaveReason_SarkicismTrait".Formatted(pawn.Named("PAWN"));
        }
    }
}
