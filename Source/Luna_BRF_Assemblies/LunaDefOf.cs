using RimWorld;
using Verse;

namespace Luna_BRF
{
    [DefOf]
    public static class LunaDefOf
    {
        static LunaDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LunaDefOf));
        }
        public static TerrainAffordanceDef Diggable;
        public static FleckDef BlastEMP;
        public static PawnKindDef BRF_PrimordialFleshBeast;
        public static PawnKindDef BRF_MiniFleshBeast;
        public static HediffDef BRF_ScarletFever;
        public static HediffDef BRF_InvisibleWatched;
        public static HediffDef BRF_RapidRegeneration;
        public static HediffDef BRF_BloodstainedTickParasiticed;
        public static DamageDef BRF_FleshPulse;
        public static PawnKindDef BRF_BloodstainedTick;
        public static PawnGroupKindDef BRF_BloodstainedTickParasiticable;
        public static PawnGroupKindDef BRF_ClumpGroup;
        public static PawnGroupKindDef BRF_CrimsonLivingFungi;
        public static AbilityDef BRF_EntityNecrophagia;
        public static AbilityDef BRF_BloodstainedTickParasitic;
        public static AbilityDef BRF_ScarletCerebralJellyfishBrainInsertion;
        public static JobDef BRF_CorpseNecrophagistDigest;
        public static JobDef BRF_AnimalResource;
        public static JobDef BRF_PlantSeed;
        public static ThingDef Filth_BRF_PrimordialTwistedFlesh;
        public static IncidentDef BRF_BloodstainedParasiticedInsectAssault;
        public static IncidentDef BRF_ClumpAssault;
    }
}
