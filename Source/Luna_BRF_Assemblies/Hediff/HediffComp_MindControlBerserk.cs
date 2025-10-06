using RimWorld;
using RimWorld.Planet;
using RimWorld.QuestGen;
using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using static RimWorld.PsychicRitualRoleDef;

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
                    if (!parent.pawn.mindState.mentalStateHandler.TryStartMentalState(Props.mentalStateDef) && Props.mandatory)
                    {
                        Pawn pawn = parent.pawn;
                        MentalStateDef stateDef = Props.mentalStateDef;
                        MentalState mentalState = (MentalState)Activator.CreateInstance(stateDef.stateClass);
                        mentalState.pawn = pawn;
                        mentalState.def = stateDef;
                        mentalState.PreStart();
                        if ((pawn.IsColonist || pawn.HostFaction == Faction.OfPlayer) && stateDef.tale != null)
                        {
                            TaleRecorder.RecordTale(stateDef.tale, pawn);
                        }
                        pawn.records.Increment(RecordDefOf.TimesInMentalState);
                        if (pawn.Drafted)
                        {
                            pawn.drafter.Drafted = false;
                        }
                        if (pawn.mechanitor != null)
                        {
                            pawn.mechanitor.UndraftAllMechs();
                        }
                        if (pawn.needs.mood != null)
                        {
                            pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
                        }
                        Lord lord = pawn.GetLord();
                        lord?.Notify_InMentalState(pawn, stateDef);
                        Thing resultingThing;
                        if (stateDef.stopsJobs && pawn.CurJob != null)
                        {
                            pawn.jobs.StopAll();
                            if (pawn.IsCarrying())
                            {
                                pawn.carryTracker.TryDropCarriedThing(pawn.PositionHeld, ThingPlaceMode.Near, out resultingThing);
                            }
                        }
                        if (pawn.Spawned)
                        {
                            pawn.Map.attackTargetsCache.UpdateTarget(pawn);
                        }
                        if (pawn.ParentHolder is CompTransporter compTransporter)
                        {
                            compTransporter.innerContainer.TryDrop(pawn, ThingPlaceMode.Near, out resultingThing);
                        }
                    }
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
