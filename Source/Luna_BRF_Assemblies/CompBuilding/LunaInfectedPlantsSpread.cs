using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;

namespace Luna_BRF
{
    public class LunaInfectedPlantsSpread : ThingComp
    {
		public CompProperties_LunaInfectedPlantsSpread Props
		{
			get
			{
				return this.props as CompProperties_LunaInfectedPlantsSpread;
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
		protected bool CellValidator(IntVec3 cell)
		{
			return (this.Props.requiredTerrain == null ||
				cell.GetTerrain(this.parent.Map) == this.Props.requiredTerrain || this.parent.Map.terrainGrid.TopTerrainAt(cell) == this.Props.requiredTerrain ) && !(cell.GetFirstBuilding(this.parent.Map) is Building_PlantGrower) && this.Props.plant.CanEverPlantAt(cell, this.parent.Map, true) 
				&& !cell.GetThingList(this.parent.Map).OfType<Plant>().Any((Plant thing) => this.Props.plant == thing.def);
		}
		protected  void DoEffect(IntVec3 cell)
		{
			(GenSpawn.Spawn(this.Props.plant, cell, this.parent.Map, WipeMode.Vanish) as Plant).Growth = 0.05f;
		}
		protected virtual bool TryGetCell(List<IntVec3> cells, out IntVec3 cell)
		{
			return cells.TryRandomElement(out cell);
		}
		protected virtual List<IntVec3> GetCells()
		{
			return (from cell in GenRadial.RadialCellsAround(this.parent.Position, this.Props.radius, true)
					where cell.InBounds(this.parent.Map) && this.CellValidator(cell)
					select cell).ToList<IntVec3>();
		}

		public int nextTickEffect;
		public CompRefuelable compFuel;
	}
}