using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompAbilityEffect_EntityFlash : CompAbilityEffect
    {
        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            base.Apply(target, dest);
            Pawn pawn = parent.pawn;
            pawn.Position = target.Cell;
            pawn.Notify_Teleported();
            FleckMaker.Static(target.CenterVector3, parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry);
            FleckMaker.Static(dest.Cell, parent.pawn.Map, FleckDefOf.PsycastSkipInnerExit);
        }
    }
}
