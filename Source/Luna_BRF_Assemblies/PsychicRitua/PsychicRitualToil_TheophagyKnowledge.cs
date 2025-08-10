using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_TheophagyKnowledge : PsychicRitualToil
    {
        public PsychicRitualRoleDef chanterRole;
        public PsychicRitualRoleDef invokerRole;

        private SimpleCurve basicKnowledgePointsFromQualityCurve;
        public PsychicRitualToil_TheophagyKnowledge(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef chanterRole, SimpleCurve basicKnowledgePointsFromQualityCurve)
        {
            this.invokerRole = invokerRole;
            this.chanterRole = chanterRole;
            this.basicKnowledgePointsFromQualityCurve = basicKnowledgePointsFromQualityCurve;
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
            PsychicRitualDef_TheophagyKnowledge psychicRitualDef_TheophagyKnowledge = (PsychicRitualDef_TheophagyKnowledge)psychicRitual.def;
            LunaBRFBaseUtility.AddFoodNeedPoints(invoker, 0.5f);
            if (!chanters.NullOrEmpty())
            {
                foreach(Pawn chanter in chanters)
                {
                    LunaBRFBaseUtility.AddFoodNeedPoints(chanter, 0.5f);
                }
            }
            bool success = true;
            float points = psychicRitualDef_TheophagyKnowledge.basicKnowledgePointsFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            List<ResearchProjectDef> anomalyResearchProjects = new List<ResearchProjectDef>();
            foreach (ResearchProjectDef allDef in DefDatabase<ResearchProjectDef>.AllDefs)
            {
                if (!allDef.IsFinished && allDef.knowledgeCategory != null && !allDef.IsHidden && allDef.CanStartNow)
                {
                    anomalyResearchProjects.Add(allDef);
                }
            }
            ResearchProjectDef project = null;
            if (!anomalyResearchProjects.NullOrEmpty())
            {
                List<ResearchProjectDef> fleshResearchProjects = new List<ResearchProjectDef>();
                bool onlyBasic = true;
                foreach (ResearchProjectDef allDef in anomalyResearchProjects)
                {
                    if (allDef.tab == LunaBRFDefOf.ResearchTabBRF_BiomancyAnomaly)
                    {
                        fleshResearchProjects.Add(allDef);
                    }
                    if (allDef.knowledgeCategory != KnowledgeCategoryDefOf.Basic)
                    {
                        onlyBasic = false;
                    }
                }
                if (!fleshResearchProjects.NullOrEmpty())
                {
                    project = fleshResearchProjects.RandomElement();
                    KnowledgeCategoryDef category = project.knowledgeCategory;
                    if(category != KnowledgeCategoryDefOf.Basic) { points *= 0.5f; }
                    AddKnowledge(project, points, 0.5f);
                }
                else
                {
                    if(!onlyBasic && ResearchProjectDefOf.AdvancedPsychicRituals.IsFinished)
                    {
                        project = fleshResearchProjects.RandomElement();
                        KnowledgeCategoryDef category = project.knowledgeCategory;
                        if (category == KnowledgeCategoryDefOf.Basic) { points *= 0.5f; }else { points *= 0.25f; }
                        AddKnowledge(project, points);
                    }
                    else if (onlyBasic)
                    {
                        project = fleshResearchProjects.RandomElement();
                        AddKnowledge(project, points/2);
                    }
                    else
                    {
                        List<ResearchProjectDef> baseResearchProjects = new List<ResearchProjectDef>();
                        foreach (ResearchProjectDef allDef in anomalyResearchProjects)
                        {
                            if (allDef.knowledgeCategory == KnowledgeCategoryDefOf.Basic)
                            {
                                baseResearchProjects.Add(allDef);
                            }
                        }
                        if (baseResearchProjects.NullOrEmpty())
                        {
                            success = false;
                        }
                        else
                        {
                            project = baseResearchProjects.RandomElement();
                            AddKnowledge(project, points / 2);
                        }
                    }
                }
            }
            else { success = false; }
            TaggedString text = "BRF_RitualTheophagy_RitualFinishText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (success ? "BRF_RitualTheophagyKnowledgeSucceeded" : "BRF_RitualTheophagyKnowledgeFailed").Translate(project.Named("PROJECT"));
            if (!success && Rand.Chance(psychicRitualDef_TheophagyKnowledge.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
            {
                text += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
                Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker);
                int duration = Mathf.RoundToInt(psychicRitualDef_TheophagyKnowledge.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
                hediff.TryGetComp<HediffComp_Disappears>()?.SetDuration(duration);
                invoker.health.AddHediff(hediff);
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
        public void AddKnowledge(ResearchProjectDef project, float points, float discount = 1f)
        {
            KnowledgeCategoryDef category = project.knowledgeCategory;
            bool flag = false;
            if (Find.ResearchManager.ApplyKnowledge(project, points, out var remainder) && remainder > 0f)
            {
                points = remainder;
                flag = true;
            }
            if (flag && category.overflowCategory != null)
            {
                Find.ResearchManager.ApplyKnowledge(category.overflowCategory, Math.Min(1f, points)*points);
            }
        }
    }
}
