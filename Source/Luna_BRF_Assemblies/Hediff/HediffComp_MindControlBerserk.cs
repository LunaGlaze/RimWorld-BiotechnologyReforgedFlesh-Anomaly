using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class HediffComp_MindControlBerserk : HediffComp
    {
        public HediffCompProperties_MindControlBerserk Props => (HediffCompProperties_MindControlBerserk)props;
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (parent.pawn.IsHashIntervalTick(60))
            {
                if(parent.pawn.mindState.mentalStateHandler.CurStateDef == Props.mentalStateDef)
                {
                    if(parent.pawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks < 128 && parent.pawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks > -1)
                    {
                        parent.pawn.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = 5120;
                    }
                }
                else
                {
                    parent.pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateDef);
                }
                if (Props.factionDef != null)
                {
                    Faction faction = Find.FactionManager.FirstFactionOfDef(Props.factionDef);
                    if (parent.pawn.Faction != faction)
                    {
                        parent.pawn.SetFaction(faction);
                    }
                }
            }
        }

    }
}
