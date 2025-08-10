using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace Luna_BRF
{
    public class PsychicRitualToil_SummonRawProphet : PsychicRitualToil
    {
        public PsychicRitualRoleDef targetRole;
        public PsychicRitualRoleDef invokerRole;
        protected PsychicRitualToil_SummonRawProphet()
        {
        }
        public PsychicRitualToil_SummonRawProphet(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
        {
            this.invokerRole = invokerRole;
            this.targetRole = targetRole;
        }
        public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph graph)
        {
            float shockDuration = ((PsychicRitualDef_SummonRawProphet)psychicRitual.def).shockDurationFromQualityCurve.Evaluate(psychicRitual.PowerPercent);
            Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(invokerRole);
            Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(targetRole);
            if (pawn != null && pawn2 != null)
            {
                ApplyOutcome(psychicRitual, pawn, pawn2, shockDuration);
            }
        }
        private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target, float shockDuration)
        {
            target.Strip(true);
            target.Kill(null,null);
            target.Corpse.Destroy();
            Map map = psychicRitual.Map;
            bool flag = false;
            if (map.mapPawns.AllPawnsSpawned != null)
            {
                List<Pawn> fleshmassNucleus = new List<Pawn>();
                foreach (Pawn item in map.mapPawns.AllPawnsSpawned)
                {
                    if (item.def == ThingDef.Named("FleshmassNucleus"))
                    {
                        fleshmassNucleus.Add(item);
                    }
                }
                if (fleshmassNucleus.Count > 0)
                {
                    Thing oblationNucleus = GenClosest.ClosestThing_Global_Reachable(invoker.Position, map, fleshmassNucleus, PathEndMode.OnCell, TraverseParms.For(invoker, canBashDoors: true, canBashFences: true) );
                    if(oblationNucleus != null)
                    {
                        flag = true;
                        oblationNucleus.Destroy();
                    }
                }
            }
            if (flag)
            {
                Find.TickManager.slower.SignalForceNormalSpeedShort();
                Faction faction = Find.FactionManager.FirstFactionOfDef(FactionDefOf.Entities);
                Pawn rawProphet = PawnGenerator.GeneratePawn(new PawnGenerationRequest(LunaBRFDefOf.BRF_RawProphet, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, 0f, 0f));
                GenSpawn.Spawn(rawProphet, target.Position, map, WipeMode.VanishOrMoveAside);
                rawProphet.health.AddHediff(HediffMaker.MakeHediff(LunaBRFDefOf.BRF_RawHeartStartHolder, rawProphet));
                rawProphet.stances.stunner.StunFor(shockDuration.SecondsToTicks(), rawProphet, false, false);
                if (ModsConfig.AnomalyActive)
                {
                    Find.EntityCodex.SetDiscovered(LunaEntityCodexEntryDefOf.BRF_RawProphet, rawProphet.def);
                }
            }
            TaggedString text = "BRF_RitualSacrificedFinishText".Translate(invoker.Named("INVOKER"), target.Named("TARGET"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag ? "BRF_SummonFleshBossSucceeded" : "BRF_SummonFleshBossFailed").Translate();
            if (!flag) { 
                text += "BRF_LackFleshmassNucleus".Translate();
                Find.PsychicRitualManager.ClearCooldown(psychicRitual.def);
            }
            Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), text, LetterDefOf.NeutralEvent);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Defs.Look(ref invokerRole, "invokerRole");
            Scribe_Defs.Look(ref targetRole, "targetRole");
        }
    }
}
