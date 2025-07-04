using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace Luna_BRF
{
    public class HediffComp_RebirthPsycorpse : HediffComp
    {
        public int ticksWithMalnutrition = 1;
        public int ticksToGetHungry;
        public int hungerTicks = 0;
        public HediffCompProperties_RebirthPsycorpse Props => (HediffCompProperties_RebirthPsycorpse)props;
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref ticksWithMalnutrition, "ticksWithMalnutrition", 1);
            Scribe_Values.Look(ref hungerTicks, "hungerTicks", 0);
            Scribe_Values.Look(ref ticksToGetHungry, "ticksToGetHungry", 0);
        }

        public override void CompPostMake()
        {
            base.CompPostMake();
            ticksWithMalnutrition = Props.malnutritionTrigger;
            ticksToGetHungry = Rand.RangeInclusive(Props.minToGetHungry, Props.maxToGetHungry);
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            hungerTicks++;
            if (hungerTicks <= ticksToGetHungry)
            {
                return;
            }
            bool malignantFleshActivation = parent.pawn.health?.hediffSet?.GetFirstHediffOfDef(LunaDefOf.BRF_MalignantFleshActivation) != null;
            if (parent.CurStage.hungerRateFactorOffset < 1.2) { parent.CurStage.hungerRateFactorOffset = 1.2f; }
            else
            {
                if (parent.pawn.IsHashIntervalTick(2500) && parent.CurStage.hungerRateFactorOffset < 3)
                {
                    parent.CurStage.hungerRateFactorOffset += 0.05f;
                    if (malignantFleshActivation)
                    {
                        parent.CurStage.hungerRateFactorOffset += 0.1f;
                    }
                }
            }
            hungerTicks = ticksToGetHungry;
            if (parent.pawn.IsHashIntervalTick(250) && (parent.pawn.health.hediffSet.HasHediff(HediffDefOf.Malnutrition) || (malignantFleshActivation && parent.CurStage.hungerRateFactorOffset >= 3)) && !parent.pawn.Downed && parent.pawn.Awake())
            {
                ticksWithMalnutrition--;
                if (malignantFleshActivation)
                {
                    ticksWithMalnutrition -= 2;
                }
                if (ticksWithMalnutrition <= 0)
                {
                    //CompTransPawnContrl(false, parent.pawn, parent.pawn.Map);
                    parent.pawn.Kill(null, null);
                }
            }
        }

        public override void Notify_PawnKilled()
        {
            CompTransPawnContrl(true, parent.pawn, parent.pawn.Map);
        }
        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            base.Pawn.health.RemoveHediff(parent);
            parent.pawn.Corpse.Destroy();
        }
        public void CompTransPawnContrl(bool died, Pawn basepawn, Map map)
        {
            if (map == null)
            {
                return;
            }
            Find.TickManager.slower.SignalForceNormalSpeedShort();
            Faction faction = Find.FactionManager.FirstFactionOfDef(Props.factionDef);
            Pawn turnPawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(Props.turnPawnKind, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, 0f, 0f));
            GenSpawn.Spawn(turnPawn, basepawn.Position, map, WipeMode.VanishOrMoveAside);
            CompInspectStringEmergence compInspectStringEmergence = turnPawn.TryGetComp<CompInspectStringEmergence>();
            if (compInspectStringEmergence != null)
            {
                compInspectStringEmergence.sourcePawn = basepawn;
            }
            if (Props.letterLabel != null && Props.letterText != null && Props.letterDef != null)
            {
                TaggedString label = Props.letterLabel.Formatted(basepawn.Named("PAWN"));
                TaggedString text = Props.letterText.Formatted(basepawn.Named("PAWN"));
                Find.LetterStack.ReceiveLetter(label, text, Props.letterDef, null, null, null, null, null, 0, playSound: true);
            }
            equipmentTransPawnContrl(basepawn, turnPawn);
            basepawn.Strip(false);
            List<Hediff> hediffs = new List<Hediff>();
            basepawn.health.hediffSet.GetHediffs(ref hediffs);
            foreach (Hediff buff in hediffs)
            {
                if (!buff.def.isBad && !buff.IsLethal && !buff.IsCurrentlyLifeThreatening && !buff.IsAnyStageLifeThreatening() && buff.def.defName != "BRF_RebirthPsycorpse_AngelsDescend" && buff.def.defName != "VoidTouched")
                {
                    Hediff buffToAdd = HediffMaker.MakeHediff(buff.def, turnPawn);
                    buffToAdd.CopyFrom(buff);
                    buffToAdd.pawn = turnPawn;
                    basepawn.health.RemoveHediff(buff);
                    turnPawn.health.AddHediff(buffToAdd);
                }
            }
            for (int i = 0; i < 20; i++)
            {
                CellFinder.TryFindRandomReachableCellNearPosition(basepawn.Position, basepawn.Position, map, 2f, TraverseParms.For(TraverseMode.NoPassClosedDoors), null, null, out var result);
                FilthMaker.TryMakeFilth(result, map, ThingDefOf.Filth_Blood);
            }
            SoundDef.Named("BRF_MeleeHit_Flesh").PlayOneShot(new TargetInfo(basepawn.Position, map));
            if (ModsConfig.AnomalyActive && Props.codexEntry != null)
            {
                Find.EntityCodex.SetDiscovered(Props.codexEntry, Props.turnPawnKind.race);
            }
            return;
        }

        public void equipmentTransPawnContrl(Pawn pawn1, Pawn pawn2)
        {
            Pawn_EquipmentTracker equipment = pawn1.equipment;
            Pawn_ApparelTracker apparel = pawn1.apparel;
            if (equipment != null)
            {
                ThingWithComps weapon = equipment.Primary;
                if (weapon != null)
                {
                    equipment.Remove(weapon);
                    pawn2.equipment.AddEquipment(weapon);
                }
            }
            if (apparel != null)
            {
                List<Apparel> weared = apparel.WornApparel.ToList();
                if (!weared.NullOrEmpty())
                {
                    foreach (Apparel appar in weared) 
                    { 
                        apparel.Remove(appar); 
                        pawn2.apparel.Wear(appar); 
                    }
                }
            }
        }
    }
}
