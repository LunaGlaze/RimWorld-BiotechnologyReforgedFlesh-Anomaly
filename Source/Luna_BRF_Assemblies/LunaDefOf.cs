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
        public static HediffDef BRF_InvisibleWatched;
        public static HediffDef BRF_RapidRegeneration;
        public static DamageDef BRF_FleshPulse;
        public static PawnGroupKindDef BRF_ClumpGroup;
        public static AbilityDef BRF_EntityNecrophagia;
        public static JobDef BRF_CorpseNecrophagistDigest;
        public static JobDef BRF_AnimalResource;
        public static ThingDef Filth_BRF_PrimordialTwistedFlesh;
    }
}
