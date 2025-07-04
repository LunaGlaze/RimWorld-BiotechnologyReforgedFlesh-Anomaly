using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class Hediff_RawHeartStartHolder : HediffWithComps
    {
        public override void Notify_PawnKilled()
        {
            if (pawn.SpawnedOrAnyParentSpawned && GenDrop.TryDropSpawn(ThingMaker.MakeThing(LunaDefOf.BRF_RawHeartStart), pawn.PositionHeld, pawn.MapHeld, ThingPlaceMode.Near, out var resultingThing))
            {
                resultingThing.SetForbidden(!resultingThing.MapHeld.areaManager.Home[resultingThing.PositionHeld]);
                string text = pawn.LabelShort;
                if (pawn.IsMutant)
                {
                    text = Find.ActiveLanguageWorker.WithDefiniteArticle(pawn.mutant.Def.label);
                }
                else if (pawn.Name == null)
                {
                    text = Find.ActiveLanguageWorker.WithDefiniteArticle(text);
                }
                Messages.Message("BRF_MessageRawHeartStartDropped".Translate(text).CapitalizeFirst(), resultingThing, MessageTypeDefOf.NeutralEvent);
            }
        }
    }
}
