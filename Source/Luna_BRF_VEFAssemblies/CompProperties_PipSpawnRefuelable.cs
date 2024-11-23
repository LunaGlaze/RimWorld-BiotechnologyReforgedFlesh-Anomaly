using System;
using RimWorld;

namespace Luna_BRF_VEFAssemblies
{
    public class CompProperties_PipSpawnRefuelable : CompProperties_Refuelable
    {
        public CompProperties_PipSpawnRefuelable()
        {
            this.compClass = typeof(LunaComPipSpawnRefuelable);
        }
        public int spawnInterval;
        public string PipeNet;
        public int spawnCount = 1;
    }
}
