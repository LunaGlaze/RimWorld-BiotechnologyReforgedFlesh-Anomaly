using Verse;

namespace Luna_BRF
{
    public class CompProperties_DamageLimitation : CompProperties
    {
        public CompProperties_DamageLimitation()
        {
            compClass = typeof(CompLuna_DamageLimitation);
        }

        public int maxSingleDamage = 100;

        public float maxEffeMultiplier = 2;

        public bool effectiveUpperLimit = true;
    }
}
