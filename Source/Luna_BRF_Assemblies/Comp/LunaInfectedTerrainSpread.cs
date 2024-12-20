using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
		public float workingSpeedMultiplier;
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
				return Find.TickManager.TicksGame + (int)(this.Props.spawnTickRate.RandomInRange / workingSpeedMultiplier);
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
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo item in base.CompGetGizmosExtra())
			{
				yield return item;
			}
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierReduced".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierReduced_Desc".Translate(),
				action = delegate
				{
					float oldSpeed = workingSpeedMultiplier;

					if (oldSpeed > 1)
					{
						workingSpeedMultiplier -= 1f;
                    }
                    else
					{
						workingSpeedMultiplier -= 0.25f;
					}
					workingSpeedMultiplier = Mathf.Clamp(workingSpeedMultiplier, 0.01f, 10f);
					this.nextTickEffect = (int)( (this.nextTickEffect - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier );
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempLower")
			};
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierReset".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierReset_Desc".Translate(),
				action = delegate
				{
					workingSpeedMultiplier = 1f;
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempReset")
			};
			yield return new Command_Action
			{
				defaultLabel = "BRF_WorkingSpeedMultiplierAdded".Translate(),
				defaultDesc = "BRF_WorkingSpeedMultiplierAdded_Desc".Translate(),
				action = delegate
				{
					float oldSpeed = workingSpeedMultiplier;

					if (oldSpeed >= 1)
					{
						workingSpeedMultiplier += 1f;
					}
					else
					{
                        switch (workingSpeedMultiplier)
                        {
							default:
								workingSpeedMultiplier = 1f;
								break;
							case 0.01f:
								workingSpeedMultiplier = 0.25f;
								break;
							case 0.25f:
								workingSpeedMultiplier = 0.5f;
								break;
							case 0.5f:
								workingSpeedMultiplier = 0.75f;
								break;
						}
					}
					workingSpeedMultiplier = Mathf.Clamp(workingSpeedMultiplier, 0.01f, 10f);
					this.nextTickEffect = (int)((this.nextTickEffect - Find.TickManager.TicksGame) * oldSpeed / workingSpeedMultiplier);
				},
				icon = ContentFinder<Texture2D>.Get("UI/Commands/TempRaise")
			};
		}
		public override void PostPostMake()
		{
			workingSpeedMultiplier = Props.baseWorkingSpeedMultiplier;
		}
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look(ref workingSpeedMultiplier, "BRF_CompWorkingSpeedMultiplier", 1f);
		}
		public override string CompInspectStringExtra()
        {
			string text = "BRF_CompWorkingSpeedMultiplier".Translate() + workingSpeedMultiplier;
			if(this.compFuel != null && this.compFuel.HasFuel)
            {
                if (!compFuel.Props.consumeFuelOnlyWhenUsed)
				{
					int numTicks = (int)((compFuel.Fuel / compFuel.Props.fuelConsumptionRate * 60000f) / workingSpeedMultiplier);
					text = text + "\n(" + "BRF_CompWorkingSpeedTrulyFuelConsume".Translate() + numTicks.ToStringTicksToPeriod() + ")";
				}
			}
			return text;

		}
		public int nextTickEffect;
		public CompRefuelable compFuel;
	}
}