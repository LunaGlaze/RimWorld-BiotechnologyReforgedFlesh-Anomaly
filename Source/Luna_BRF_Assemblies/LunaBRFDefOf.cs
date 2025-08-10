using RimWorld;
using Verse;

namespace Luna_BRF
{
    [DefOf]
    public static class LunaBRFDefOf
    {
        static LunaBRFDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LunaBRFDefOf));
        }
        public static TerrainAffordanceDef Diggable;
        public static TerrainAffordanceDef BRF_FleshBlanket;
        public static FleckDef BlastEMP;
        public static HediffDef BRF_ScarletFever;
        public static HediffDef BRF_MalignantFleshActivation;
        public static HediffDef BRF_InvisibleWatched;
        public static HediffDef BRF_RapidRegeneration;
        public static HediffDef BRF_BloodstainedTickParasiticed;
        public static HediffDef BRF_RebirthPsycorpse_AngelsDescend;
        public static HediffDef BRF_RawHeartStartHolder;
        public static HediffDef BRF_Akuloth;
        public static HediffDef BRF_SatisfactionAmpulla;
        public static HediffDef BRF_ThirstAmpulla;
        public static DamageDef BRF_FleshPulse;
        public static PawnKindDef BRF_Clump;
        public static PawnKindDef BRF_BloodstainedTick;
        public static PawnKindDef BRF_ThornbladeMaggot;
        public static PawnKindDef BRF_BloodvineZombie;
        public static PawnKindDef BRF_PrimordialFleshBeast;
        public static PawnKindDef BRF_MiniFleshBeast;
        public static PawnKindDef BRF_RebirthPsycorpse;
        public static PawnKindDef BRF_RawProphet;
        public static PawnGroupKindDef BRF_BloodvineZombieSwarmer;
        public static PawnGroupKindDef BRF_ThornbladeMaggotLarvalSwarmer;
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
        public static ThingDef BRF_FishySteamGas;
        public static IncidentDef BRF_BloodstainedParasiticedInsectAssault;
        public static IncidentDef BRF_ClumpAssault;
        public static ThingDef BRF_RawHeartStart;
        public static ThingDef BRF_PsycorpseGuard;
        public static TraitDef BRF_Sarkicism;
        public static ThoughtDef BRF_RebirthPsycorpseReplace;
        public static MemeDef Inhuman;
        public static ResearchTabDef ResearchTabBRF_BiomancyAnomaly;
    }
}
