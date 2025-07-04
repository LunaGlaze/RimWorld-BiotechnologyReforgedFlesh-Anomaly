using RimWorld;
using Verse;

namespace Luna_BRF
{
    [DefOf]
    public static class LunaEntityCodexEntryDefOf
    {
        static LunaEntityCodexEntryDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LunaDefOf));
        }
        public static EntityCodexEntryDef BRF_BloodstainedTick;
        public static EntityCodexEntryDef BRF_ThornbladeMaggot;
        public static EntityCodexEntryDef BRF_BloodvineZombie;
        public static EntityCodexEntryDef BRF_Clump;
        public static EntityCodexEntryDef BRF_CrimsonLivingFungi;
        public static EntityCodexEntryDef BRF_ScarletCerebralJellyfish;
        public static EntityCodexEntryDef BRF_RawProphet;
    }
}
