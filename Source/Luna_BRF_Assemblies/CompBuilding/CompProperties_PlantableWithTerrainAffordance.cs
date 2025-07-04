using RimWorld;
using Verse;

namespace Luna_BRF
{
    public class CompProperties_PlantableWithTerrainAffordance : CompProperties_Plantable
    {
        public TerrainAffordanceDef terrainAffordanceNeeded;

        public CompProperties_PlantableWithTerrainAffordance()
        {
            compClass = typeof(CompPlantableWithTerrainAffordance);
        }
    }
}
