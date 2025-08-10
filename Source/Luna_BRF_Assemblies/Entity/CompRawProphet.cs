using RimWorld;
using UnityEngine;
using Verse;

namespace Luna_BRF
{
    public class CompRawProphet : ThingComp
    {
        private static readonly IntRange ClumpSpawnCountRange = new IntRange(0, 3);

        private static readonly IntRange PrimordialFleshBeastSpawnRange = new IntRange(2, 4);

        private static readonly IntRange BloodFilthCountRange = new IntRange(1, 2);

        private const int DamageSpawnThresholds = 250;

        private static readonly FloatRange HpRegenerationPointsRange = new FloatRange(1, 75);

        private float totalDamageTaken;
        public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            float num = totalDamageTaken;
            totalDamageTaken += totalDamageDealt;
            if (num == 0f)
            {
                int randomInRangeA = PrimordialFleshBeastSpawnRange.RandomInRange;
                for (int i = 0; i < randomInRangeA; i++)
                {
                    Pawn pawn1 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(LunaBRFDefOf.BRF_PrimordialFleshBeast, parent.Faction, PawnGenerationContext.NonPlayer, -1, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, 0f));
                    SpawnPawn(pawn1, parent.PositionHeld, parent.MapHeld);
                }
                FleshbeastUtility.MeatSplatter(BloodFilthCountRange.RandomInRange, parent.PositionHeld, parent.MapHeld);
                FilthMaker.TryMakeFilth(parent.PositionHeld, parent.MapHeld, LunaBRFDefOf.Filth_BRF_PrimordialTwistedFlesh);
                if (parent is Pawn pawn00)
                {
                    float point = HpRegenerationPointsRange.RandomInRange + 25;
                    LunaBRFHediffUtility.AddPointRapidRegeneration(pawn00, point);
                }
            }
            if ((int)num / DamageSpawnThresholds >= (int)totalDamageTaken / DamageSpawnThresholds)
            {
                return;
            }
            int randomInRangeB = ClumpSpawnCountRange.RandomInRange;
            if(num < DamageSpawnThresholds) { randomInRangeB = Mathf.Max(1, randomInRangeB); }
            for (int i = 0; i < randomInRangeB; i++)
            {
                Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(LunaBRFDefOf.BRF_Clump, parent.Faction, PawnGenerationContext.NonPlayer, -1, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, 0f));
                SpawnPawn(pawn2, parent.PositionHeld, parent.MapHeld);
            }
            FleshbeastUtility.MeatSplatter(BloodFilthCountRange.RandomInRange, parent.PositionHeld, parent.MapHeld);
            FilthMaker.TryMakeFilth(parent.PositionHeld, parent.MapHeld, LunaBRFDefOf.Filth_BRF_PrimordialTwistedFlesh);
            if (parent.Map != null)
            {
                foreach (IntVec3 cell in GenRadial.RadialCellsAround(parent.Position, 8, true))
                {
                    if (cell.InBounds(parent.Map))
                    {
                        Thing thing = ThingMaker.MakeThing(LunaBRFDefOf.BRF_FishySteamGas);
                        thing.Rotation = Rot4.North;
                        thing.Position = cell;
                        thing.SpawnSetup(parent.Map, false);
                    }
                }
            }
            if (Rand.Bool && parent is Pawn pawn0)
            {
                float point = HpRegenerationPointsRange.RandomInRange;
                LunaBRFHediffUtility.AddPointRapidRegeneration(pawn0, point);
            }
        }
        private void SpawnPawn(Pawn pawn, IntVec3 position, Map map)
        {
            GenSpawn.Spawn(pawn, position, map, WipeMode.VanishOrMoveAside);
            FleshbeastUtility.SpawnPawnAsFlyer(pawn, map, position);
        }
        public override void PostExposeData()
        {
            Scribe_Values.Look(ref totalDamageTaken, "totalDamageTaken", 0f);
        }
    }
}
