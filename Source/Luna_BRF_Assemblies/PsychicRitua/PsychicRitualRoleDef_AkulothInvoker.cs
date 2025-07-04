using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class PsychicRitualRoleDef_AkulothInvoker : PsychicRitualRoleDef
    {
        protected override bool PawnCanDo(Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
        {
            if (!base.PawnCanDo(context, pawn, target, out reason))
            {
                return false;
            }
            Hediff firstHediff = pawn.health.hediffSet.GetFirstHediffOfDef(LunaDefOf.BRF_Akuloth);
            if (firstHediff != null)
            {
                return false;
            }
            return true;
        }
        public override TaggedString PawnCannotDoReason(AnyEnum reason, Context context, Pawn pawn, TargetInfo target)
        {
            return "BRF_psychicRitualLeaveReason_HasAkuloth".Formatted(pawn.Named("PAWN"));
        }
    }
}
