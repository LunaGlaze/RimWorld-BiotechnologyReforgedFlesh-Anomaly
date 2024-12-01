using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Luna_BRF
{
    public class LunaInfectedTerrainSpread : ThingComp
    {
		public CompProperties_LunaInfectedTerrainSpread Props
		{
			get
			{
				return this.props as CompProperties_LunaInfectedTerrainSpread;
			}
		}
		protected bool Active
		{
			get
			{
				return this.parent.Spawned && (this.compFuel == null || this.compFuel.HasFuel);
			}
		}
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				if (this.nextTickEffect == 0)
				{
					this.nextTickEffect = this.NextTickEffect;
				}
				if (Find.TickManager.TicksGame >= this.nextTickEffect)
				{
					List<IntVec3> cells = this.GetCells();
					IntVec3 cell;
					if (this.TryGetCell(cells, out cell))
					{
						this.DoEffect(cell);
					}
					this.nextTickEffect = this.NextTickEffect;
					return;
				}
			}
			else
			{
				this.nextTickEffect++;
			}
		}
		public int NextTickEffect
		{
			get
			{
				return Find.TickManager.TicksGame + this.Props.spawnTickRate.RandomInRange;
			}
		}
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.terrainGrid.SetTerrain(this.parent.Position, this.Props.terrainToSet);
			}
			this.compFuel = this.parent.GetComp<CompRefuelable>();
		}
		protected bool CellValidator(IntVec3 cell)
		{
			TerrainDef terrain = cell.GetTerrain(this.parent.Map);
			if (terrain != null && terrain != this.Props.terrainToSet && 
				((this.Props.requiredTerrain == null && (LunaInfectedTerrainSpread.TerrainValidator(terrain) || AllowTranTerrain(terrain) )) || this.Props.requiredTerrain == terrain))
			{
				Building edifice = cell.GetEdifice(this.parent.Map);
				return edifice == null || (!edifice.def.building.isNaturalRock && !edifice.def.building.isResourceRock);
			}
			return false;
		}
		public static bool TerrainValidator(TerrainDef terrain)
		{
			return !terrain.IsWater && !terrain.layerable && 
				(terrain.natural || (terrain.affordances != null && (terrain.affordances.Contains(TerrainAffordanceDefOf.SmoothableStone) || 
				terrain.affordances.Contains(LunaDefOf.Diggable)))) && !terrain.HasTag("Road");
		}
		public bool AllowTranTerrain(TerrainDef terrain)
		{
			List<TerrainDef> terrainDefs = null;
			if (Props.allowTerrains == null)
            {
				return false;
            }
            else
            {
				terrainDefs = Props.allowTerrains;
				return terrainDefs.Contains(terrain);
			}
		}
		protected List<IntVec3> GetCells()
		{
			return (from cell in GenRadial.RadialCellsAround(this.parent.Position, this.Props.radius, true)
					where cell.InBounds(this.parent.Map) && this.CellValidator(cell)
					select cell).ToList<IntVec3>();
		}
		protected bool TryGetCell(List<IntVec3> cells, out IntVec3 cell)
		{
			cell = (from x in cells
					orderby x.DistanceTo(this.parent.Position)
					select x).FirstOrDefault<IntVec3>();
			return cell != default(IntVec3);
		}
		protected void DoEffect(IntVec3 cell)
		{
			this.parent.Map.terrainGrid.SetTerrain(cell, this.Props.terrainToSet);
		}

		public int nextTickEffect;
		public CompRefuelable compFuel;
	}
}