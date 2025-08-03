using Verse;

namespace Luna_BRF
{
    public class PlaceWorker_BRF_FleshBlanket : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            foreach (IntVec3 item in GenAdj.CellsOccupiedBy(loc, rot, checkingDef.Size))
            {
                if (!map.terrainGrid.TerrainAt(item).affordances.NotNullAndContains(LunaDefOf.BRF_FleshBlanket))
                {
                    TerrainDef top = map.terrainGrid.TopTerrainAt(item);
                    TerrainDef foundation = map.terrainGrid.FoundationAt(item);
                    bool falg_top = top != null && top.affordances.NotNullAndContains(LunaDefOf.BRF_FleshBlanket);
                    bool falg_foundation = foundation != null && foundation.affordances.NotNullAndContains(LunaDefOf.BRF_FleshBlanket);
                    if(!falg_top && !falg_foundation)
                    {
                        return new AcceptanceReport("TerrainCannotSupport_TerrainAffordance".Translate(checkingDef, LunaDefOf.BRF_FleshBlanket));
                    }
                }
            }
            return true;
        }
    }
}
