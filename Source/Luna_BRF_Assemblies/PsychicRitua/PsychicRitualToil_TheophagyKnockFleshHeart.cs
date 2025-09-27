using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Noise;

namespace Luna_BRF
{
    public class PsychicRitualToil_TheophagyKnockFleshHeart : PsychicRitualToil
    {
        public PsychicRitualRoleDef chanterRole;
        public PsychicRitualRoleDef invokerRole;
        public SimpleCurve psychicShockChanceFromQualityCurve;

        public static readonly LargeBuildingSpawnParms HeartSpawnParms = new LargeBuildingSpawnParms
        {
            minDistToEdge = 10,
            minDistanceToColonyBuilding = 15f,
            preferFarFromColony = true
        };
        public PsychicRitualToil_TheophagyKnockFleshHeart(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef chanterRole, SimpleCurve psychicShockChanceFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.chanterRole = chanterRole;
            this.psychicShockChanceFromQualityCurve = psychicShockChanceFromQualityCurve;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
        {
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            List<Pawn> chanters = psychicRitual.assignments.AssignedPawns(chanterRole).ToList();
            if (pawn != null)
            {
                ApplyOutcome(psychicRitual, pawn, chanters);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, List<Pawn> chanters)
        {
            PsychicRitualDef_TheophagyKnockFleshHeart psychicRitualDef_TheophagyKnockFleshHeart = (PsychicRitualDef_TheophagyKnockFleshHeart)psychicRitual.def;
            chanters.Add(invoker);
            if (!chanters.NullOrEmpty())
            {
                foreach (Pawn chanter in chanters)
                {
                    LunaBRFBaseUtility.AddFoodNeedPoints(chanter, 0.5f);
                }
            }
            bool flag = false;
            if (LargeBuildingCellFinder.TryFindCell(out var cell, psychicRitual.Map, HeartSpawnParms.ForThing(ThingDefOf.FleshmassHeart)))
            {
                flag = true;
            }
            IncidentParms parms = new IncidentParms();
            BuildingGroundSpawner buildingGroundSpawner = new BuildingGroundSpawner();
            if (flag) {
                parms.target = psychicRitual.Map;
                parms.points = psychicRitualDef_TheophagyKnockFleshHeart.fleshmassheartPointsFromThreatPointsCurve.Evaluate(psychicRitual.PowerPercent) * Find.Storyteller.difficulty.threatScale;
                parms.faction = Faction.OfEntities;
                buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.FleshmassHeartSpawner);
                Building_FleshmassHeart obj = buildingGroundSpawner.ThingToSpawn as Building_FleshmassHeart;
                obj.Comp.threatPoints = parms.points;
                obj.SetFaction(Faction.OfEntities);
                GenSpawn.Spawn(buildingGroundSpawner, cell, psychicRitual.Map);
            }
            //虚空扰动同款结算
            TaggedString text = "BRF_RitualTheophagy_RitualFinishText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag ? "BRF_TheophagyKnockFleshHeartSucceeded" : "BRF_TheophagyKnockFleshHeartFailed").Translate();
            if (!flag || Rand.Chance(psychicRitualDef_TheophagyKnockFleshHeart.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
            {
                text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                int duration = Mathf.RoundToInt(psychicRitualDef_TheophagyKnockFleshHeart.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                invoker.health.AddHediff(hediff);
            }
            if (flag)
            {
                IncidentWorker.SendIncidentLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent, parms, buildingGroundSpawner, LunaBRFDefOf.FleshmassHeart);
            }
            else
            {
                Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
            }
        }
    }
}
