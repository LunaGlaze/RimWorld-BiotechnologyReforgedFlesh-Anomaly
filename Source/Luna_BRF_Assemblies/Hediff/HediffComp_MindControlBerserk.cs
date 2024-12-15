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
                parent.pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateDef);
                if (Props.factionDef != null)
                {
                    Faction faction = Find.FactionManager.FirstFactionOfDef(Props.factionDef);
                    if (parent.pawn.Faction != faction)
                    {
                        parent.pawn.SetFaction(faction);
                    }
                }
                else
                {
                    parent.pawn.SetFaction(Faction.OfEntities);
                }
            }
        }

    }
}
