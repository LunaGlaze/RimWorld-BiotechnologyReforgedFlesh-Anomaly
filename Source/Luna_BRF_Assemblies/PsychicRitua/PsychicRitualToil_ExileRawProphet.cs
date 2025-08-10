using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_ExileRawProphet : PsychicRitualToil
    {
        public PsychicRitualRoleDef targetRole;
        public PsychicRitualRoleDef invokerRole;
        public SimpleCurve psychicShockChanceFromQualityCurve;
        protected PsychicRitualToil_ExileRawProphet()
        {
        }
        public PsychicRitualToil_ExileRawProphet(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole, SimpleCurve psychicShockChanceFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
            this.psychicShockChanceFromQualityCurve = psychicShockChanceFromQualityCurve;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
        {
            target.Strip(true);
            target.Kill(null, null);
            target.Corpse.Destroy();
            bool flag = false;
            PsychicRitualDef_ExileRawProphet psychicRitualDef_ExileRawProphet = (PsychicRitualDef_ExileRawProphet)psychicRitual.def;
            Map map = psychicRitual.Map;
            TaggedString text = "null";
            if (ModsConfig.AnomalyActive && map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness))
            {
                text = "BRF_RitualSacrificedFinishText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL")) + "\n\n" + "BRF_RitualSacrificedFailedUnnaturalDarkness".Translate();
                if (Rand.Chance(psychicRitualDef_ExileRawProphet.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
                {
                    text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                    int duration = Mathf.RoundToInt(psychicRitualDef_ExileRawProphet.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                    hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                    invoker.health.AddHediff(hediff);
                }
                Find.PsychicRitualManager.ClearCooldown(psychicRitual.def);
            }
            else
            {
                if (map.mapPawns.AllPawnsSpawned != null)
                {
                    List<Pawn> rawProphet = new List<Pawn>();
                    foreach (Pawn item in map.mapPawns.AllPawnsSpawned)
                    {
                        if (item.def == ThingDef.Named("BRF_RawProphet"))
                        {
                            rawProphet.Add(item);
                        }
                    }
                    if (!rawProphet.NullOrEmpty())
                    {
                        foreach (Pawn item in rawProphet)
                        {
                            CompHoldingPlatformTarget compHoldingPlatformTarget = item.TryGetComp<CompHoldingPlatformTarget>();
                            if (compHoldingPlatformTarget == null || compHoldingPlatformTarget.targetHolder == null)
                            {
                                if (item.health.hediffSet.HasHediff(LunaBRFDefOf.BRF_RawHeartStartHolder))
                                {
                                    if (item.health.Downed || item.health.summaryHealth.SummaryHealthPercent < 0.5)
                                    {
                                        if (item.SpawnedOrAnyParentSpawned && GenDrop.TryDropSpawn(ThingMaker.MakeThing(LunaBRFDefOf.BRF_RawHeartStart), item.PositionHeld, item.MapHeld, ThingPlaceMode.Near, out var resultingThing))
                                        {
                                            resultingThing.SetForbidden(!resultingThing.MapHeld.areaManager.Home[resultingThing.PositionHeld]);
                                            string textaaa = item.LabelShort;
                                            if (item.IsMutant)
                                            {
                                                textaaa = Find.ActiveLanguageWorker.WithDefiniteArticle(item.mutant.Def.label);
                                            }
                                            else if (item.Name == null)
                                            {
                                                textaaa = Find.ActiveLanguageWorker.WithDefiniteArticle(textaaa);
                                            }
                                            Messages.Message("BRF_MessageRawHeartStartDropped".Translate(textaaa).CapitalizeFirst(), resultingThing, MessageTypeDefOf.NeutralEvent);
                                        }
                                    }
                                }
                                item.Destroy();
                            }
                        }
                        flag = true;
                    }
                }
                if (map != null)
                {
                    List<Thing> corpses = map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
                        .Where(thing => thing is Corpse corpse && corpse.InnerPawn != null && corpse.InnerPawn.def.defName == "BRF_RawProphet").ToList();
                    if (!corpses.NullOrEmpty())
                    {
                        foreach (Thing item in corpses)
                        {
                            if (item is Corpse corpse && corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.DeathRefusal))
                            {
                                item.Destroy();
                            }
                        }
                        flag = true;
                    }
                }
                //虚空扰动同款结算
                text = "BRF_RitualSacrificedFinishText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag ? "BRF_ExileRawProphetSucceeded" : "BRF_ExileRawProphetFailed").Translate();
                if (Rand.Chance(psychicRitualDef_ExileRawProphet.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
                {
                    text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                    Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                    int duration = Mathf.RoundToInt(psychicRitualDef_ExileRawProphet.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                    hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                    invoker.health.AddHediff(hediff);
                }
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Values.Look(ref targetRole, "targetRole");
        }
    }
}
